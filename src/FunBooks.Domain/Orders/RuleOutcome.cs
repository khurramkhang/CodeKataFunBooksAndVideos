namespace FunBooks.Domain.Orders;

public enum RuleOutcomeStatus
{
    Applied,
    Skipped,
    Failed,
}


public sealed record RuleOutcome(string RuleName, RuleOutcomeStatus Status, string Detail)
{
    public static RuleOutcome Applied(string ruleName, string detail) => new(ruleName, RuleOutcomeStatus.Applied, detail);
    public static RuleOutcome Skipped(string ruleName, string detail) => new(ruleName, RuleOutcomeStatus.Skipped, detail);
    public static RuleOutcome Failed(string ruleName, string detail) => new(ruleName, RuleOutcomeStatus.Failed, detail);
}
