using FunBooks.Domain.Common;

namespace FunBooks.Domain.Customers;


public readonly record struct MembershipChange(MembershipType Before, MembershipType After)
{
    public bool Changed => Before != After;
}

public sealed class Customer
{
    private readonly Lock _sync = new();
    private MembershipType _memberships;

    public Customer(long id, string name, string email, Address shippingAddress, MembershipType memberships = MembershipType.None)
    {
        //ToDo:Vlidations
        Id = id;
        Name = name;
        Email = email;
        ShippingAddress = shippingAddress;
        _memberships = memberships;
    }

    public long Id { get; }

    public string Name { get; }

    public string Email { get; }

    public Address ShippingAddress { get; }

    public MembershipType Memberships
    {
        get
        {
            lock (_sync)
            {
                return _memberships;
            }
        }
    }

    public bool HasMembership(MembershipType type) =>
        type != MembershipType.None && (Memberships & type) == type;

    
    public MembershipChange ActivateMembership(MembershipType type)
    {
        //ToDo:Validations   
        lock (_sync)
        {
            var before = _memberships;
            _memberships |= type;
            return new MembershipChange(before, _memberships);
        }
    }
}
