using FunBooks.Domain.Catalog;
using FunBooks.Domain.Common;

namespace FunBooks.Domain.Orders;

public enum OrderStatus
{
    Created,
    Processed,
    Failed,
}

public sealed class PurchaseOrder
{
    public const int MaxLines = 50;

    private readonly List<RuleOutcome> _outcomes = [];

    private PurchaseOrder(long id, long customerId, IReadOnlyList<OrderLine> lines, Money total, DateTimeOffset createdAt)
    {
        Id = id;
        CustomerId = customerId;
        Lines = lines;
        Total = total;
        CreatedAt = createdAt;
        Status = OrderStatus.Created;
    }

    public long Id { get; }

    public long CustomerId { get; }

    public IReadOnlyList<OrderLine> Lines { get; }

    public Money Total { get; }

    public OrderStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; }

    public DateTimeOffset? ProcessedAt { get; private set; }

    public string? ShippingSlipId { get; private set; }

    public IReadOnlyList<RuleOutcome> Outcomes => _outcomes;

    public bool ContainsPhysicalItems => Lines.Any(line => line.Item.IsPhysical);

    public IEnumerable<OrderLine> PhysicalLines => Lines.Where(line => line.Item.IsPhysical);

    public IEnumerable<MembershipProduct> Memberships => Lines.Select(line => line.Item).OfType<MembershipProduct>();

    public static PurchaseOrder Create(long id, long customerId, IEnumerable<OrderLine> lines, DateTimeOffset createdAt)
    {
        //ToDo: Validations
        var lineList = lines.ToList();

        if (lineList.Count == 0)
        {
            throw new DomainRuleException("order.empty", "A purchase order must contain at least one item line.");
        }

        if (lineList.Count > MaxLines)
        {
            throw new DomainRuleException("order.too_many_lines", $"A purchase order cannot have more than {MaxLines} item lines.");
        }

        //ToDo: Duplicate memberships?

        var total = lineList.Aggregate(Money.Zero(), (sum, line) => sum + line.LineTotal);

        return new PurchaseOrder(id, customerId, lineList.AsReadOnly(), total, createdAt);
    }

    public void AttachShippingSlip(string shippingSlipId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(shippingSlipId);
        EnsureNotFinished();
        ShippingSlipId = shippingSlipId;
    }

    public void MarkProcessed(IEnumerable<RuleOutcome> outcomes, DateTimeOffset processedAt) =>
        Finish(OrderStatus.Processed, outcomes, processedAt);

    public void MarkFailed(IEnumerable<RuleOutcome> outcomes, DateTimeOffset processedAt) =>
        Finish(OrderStatus.Failed, outcomes, processedAt);

    private void Finish(OrderStatus status, IEnumerable<RuleOutcome> outcomes, DateTimeOffset processedAt)
    {
        ArgumentNullException.ThrowIfNull(outcomes);
        EnsureNotFinished();

        _outcomes.AddRange(outcomes);
        Status = status;
        ProcessedAt = processedAt;
    }

    private void EnsureNotFinished()
    {
        if (Status != OrderStatus.Created)
        {
            throw new InvalidOperationException($"Purchase order {Id} has already been processed.");
        }
    }
}
