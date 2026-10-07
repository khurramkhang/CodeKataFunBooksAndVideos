using FunBooks.Application.Abstractions;
using FunBooks.Domain.Orders;
using FunBooks.Domain.Shipping;

namespace FunBooks.Application.Processing.Rules;





public sealed class GenerateShippingSlipRule(IShippingSlipRepository shippingSlips, TimeProvider timeProvider) : IPurchaseOrderRule
{
    public string Name => "BR2";

    public string Description => "Generate a shipping slip for the physical products in the order.";

    public int Order => 200;

    public bool AppliesTo(OrderProcessingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Order.ContainsPhysicalItems;
    }

    public async Task<RuleOutcome> ApplyAsync(OrderProcessingContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        var slip = ShippingSlip.CreateFor(shippingSlips.NextId(), context.Order, context.Customer, timeProvider.GetUtcNow());

        await shippingSlips.AddAsync(slip, cancellationToken).ConfigureAwait(false);

        context.Order.AttachShippingSlip(slip.Id);
        context.ShippingSlip = slip;

        var itemCount = slip.Items.Sum(item => item.Quantity);
        return RuleOutcome.Applied(Name, $"Shipping slip {slip.Id} created with {itemCount} item(s).");
    }
}
