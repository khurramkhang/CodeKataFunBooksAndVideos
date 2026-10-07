using System.Diagnostics;
using FunBooks.Domain.Customers;
using FunBooks.Domain.Orders;
using Microsoft.Extensions.Logging;

namespace FunBooks.Application.Processing;

public interface IPurchaseOrderProcessor
{
    Task<OrderProcessingResult> ProcessAsync(PurchaseOrder order, Customer customer, CancellationToken cancellationToken);
}











public sealed partial class PurchaseOrderProcessor : IPurchaseOrderProcessor
{
    private readonly IReadOnlyList<IPurchaseOrderRule> _rules;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<PurchaseOrderProcessor> _logger;

    public PurchaseOrderProcessor(
        IEnumerable<IPurchaseOrderRule> rules,
        TimeProvider timeProvider,
        ILogger<PurchaseOrderProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(rules);

        _rules = rules
            .OrderBy(rule => rule.Order)
            .ThenBy(rule => rule.Name, StringComparer.Ordinal)
            .ToArray();

        var duplicate = _rules
            .GroupBy(rule => rule.Name, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicate is not null)
        {
            throw new InvalidOperationException($"Two business rules are registered with the name '{duplicate.Key}'.");
        }

        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task<OrderProcessingResult> ProcessAsync(PurchaseOrder order, Customer customer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(customer);

        using var activity = new Activity("ProcessPurchaseOrder");
        activity?.SetTag("order.id", order.Id);
        activity?.SetTag("order.line_count", order.Lines.Count);

        var startedAt = _timeProvider.GetTimestamp();
        var context = new OrderProcessingContext(order, customer);
        var outcomes = new List<RuleOutcome>(_rules.Count);

        LogProcessingStarted(order.Id, order.Lines.Count, _rules.Count);

        foreach (var rule in _rules)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!rule.AppliesTo(context))
            {
                outcomes.Add(RuleOutcome.Skipped(rule.Name, "Rule does not apply to this order."));
                LogRuleSkipped(rule.Name, order.Id);
                continue;
            }

            var outcome = await RunRuleAsync(rule, context, cancellationToken).ConfigureAwait(false);
            outcomes.Add(outcome);

            if (outcome.Status == RuleOutcomeStatus.Failed)
            {
                return Finish(order, context, outcomes, startedAt, activity, succeeded: false);
            }
        }

        return Finish(order, context, outcomes, startedAt, activity, succeeded: true);
    }

    private async Task<RuleOutcome> RunRuleAsync(IPurchaseOrderRule rule, OrderProcessingContext context, CancellationToken cancellationToken)
    {
        using var ruleActivity =  new Activity($"Rule {rule.Name}");
        ruleActivity?.SetTag("rule.name", rule.Name);

        try
        {
            var outcome = await rule.ApplyAsync(context, cancellationToken).ConfigureAwait(false);

            if (outcome.Status == RuleOutcomeStatus.Failed)
            {
                ruleActivity?.SetStatus(ActivityStatusCode.Error, outcome.Detail);
                LogRuleReportedFailure(rule.Name, context.Order.Id, outcome.Detail);
            }
            else
            {
                LogRuleCompleted(rule.Name, context.Order.Id, outcome.Status, outcome.Detail);
            }

            return outcome;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            ruleActivity?.AddException(exception);
            ruleActivity?.SetStatus(ActivityStatusCode.Error, exception.GetType().Name);
            LogRuleThrew(exception, rule.Name, context.Order.Id);

            
            return RuleOutcome.Failed(rule.Name, "The rule could not be completed. The error has been logged.");
        }
    }

    private OrderProcessingResult Finish(
        PurchaseOrder order,
        OrderProcessingContext context,
        List<RuleOutcome> outcomes,
        long startedAt,
        Activity? activity,
        bool succeeded)
    {
        var now = _timeProvider.GetUtcNow();

        if (succeeded)
        {
            order.MarkProcessed(outcomes, now);
        }
        else
        {
            order.MarkFailed(outcomes, now);
            activity?.SetStatus(ActivityStatusCode.Error, "A business rule failed.");
        }

        var elapsed = _timeProvider.GetElapsedTime(startedAt);
        activity?.SetTag("order.status", order.Status.ToString());
        LogProcessingFinished(order.Id, order.Status, elapsed.TotalMilliseconds);

        return new OrderProcessingResult(succeeded, outcomes.AsReadOnly(), context.ShippingSlip);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Processing purchase order {OrderId} with {LineCount} lines through {RuleCount} rules")]
    private partial void LogProcessingStarted(long orderId, int lineCount, int ruleCount);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Rule {RuleName} skipped for order {OrderId}")]
    private partial void LogRuleSkipped(string ruleName, long orderId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Rule {RuleName} finished for order {OrderId} with {Status}: {Detail}")]
    private partial void LogRuleCompleted(string ruleName, long orderId, RuleOutcomeStatus status, string detail);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Rule {RuleName} reported a failure for order {OrderId}: {Detail}")]
    private partial void LogRuleReportedFailure(string ruleName, long orderId, string detail);

    [LoggerMessage(Level = LogLevel.Error, Message = "Rule {RuleName} threw an exception for order {OrderId}")]
    private partial void LogRuleThrew(Exception exception, string ruleName, long orderId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Purchase order {OrderId} finished with status {Status} in {ElapsedMs:0.0} ms")]
    private partial void LogProcessingFinished(long orderId, OrderStatus status, double elapsedMs);
}
