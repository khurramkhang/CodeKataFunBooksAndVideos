using FunBooks.Domain.Common;
using FunBooks.Domain.Customers;

namespace FunBooks.Domain.Catalog;


public sealed class MembershipProduct : CatalogItem
{
    public MembershipProduct(string id, string name, MembershipType membershipType, Money price)
        : base(id, name, price)
    {
        MembershipType = membershipType;
    }

    public MembershipType MembershipType { get; }

    public override bool IsPhysical => false;

    public override ProductKind Kind => ProductKind.Membership;
}
