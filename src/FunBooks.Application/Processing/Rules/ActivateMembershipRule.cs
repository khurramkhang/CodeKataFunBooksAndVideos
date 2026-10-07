using FunBooks.Application.Abstractions;
using FunBooks.Domain.Customers;
using FunBooks.Domain.Orders;

namespace FunBooks.Application.Processing.Rules;





public sealed class ActivateMembershipRule(ICustomerRepository customers) : IPurchaseOrderRule
{
    public string Name => "BR1";

    public string Description => "Activate purchased memberships on the customer account immediately.";

    public int Order => 100;

    public bool AppliesTo(OrderProcessingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Order.Memberships.Any();
    }

    public async Task<RuleOutcome> ApplyAsync(OrderProcessingContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        var activated = new List<string>();
        var alreadyActive = new List<string>();

        foreach (var membership in context.Order.Memberships)
        {
            var change = context.Customer.ActivateMembership(membership.MembershipType);
            (change.Changed ? activated : alreadyActive).Add(membership.MembershipType.ToString());
        }

        await customers.SaveAsync(context.Customer, cancellationToken).ConfigureAwait(false);

        var detail = Describe(activated, alreadyActive, context.Customer.Memberships);
        return RuleOutcome.Applied(Name, detail);
    }

    private static string Describe(List<string> activated, List<string> alreadyActive, MembershipType now)
    {
        var parts = new List<string>();

        if (activated.Count > 0)
        {
            parts.Add($"Activated {string.Join(", ", activated)}.");
        }

        if (alreadyActive.Count > 0)
        {
            parts.Add($"Already active: {string.Join(", ", alreadyActive)}.");
        }

        parts.Add($"Customer memberships are now {now}.");
        return string.Join(' ', parts);
    }
}
