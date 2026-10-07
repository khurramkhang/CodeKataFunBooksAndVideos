using FunBooks.Domain.Common;
using FunBooks.Domain.Customers;
using FunBooks.UnitTests.TestSupport;

namespace FunBooks.UnitTests.Domain;

public class CustomerTests
{
    [Fact]
    public void New_customer_has_no_memberships()
    {
        var customer = TestData.Customer();

        Assert.Equal(MembershipType.None, customer.Memberships);
        Assert.False(customer.HasMembership(MembershipType.BookClub));
    }

    [Fact]
    public void Activating_book_club_gives_book_club()
    {
        var customer = TestData.Customer();

        var change = customer.ActivateMembership(MembershipType.BookClub);

        Assert.True(change.Changed);
        Assert.Equal(MembershipType.BookClub, customer.Memberships);
        Assert.True(customer.HasMembership(MembershipType.BookClub));
        Assert.False(customer.HasMembership(MembershipType.Premium));
    }

    [Theory]
    [InlineData(MembershipType.BookClub, MembershipType.VideoClub)]
    [InlineData(MembershipType.VideoClub, MembershipType.BookClub)]
    public void Joining_both_clubs_makes_the_customer_premium(MembershipType first, MembershipType second)
    {
        var customer = TestData.Customer();

        customer.ActivateMembership(first);
        customer.ActivateMembership(second);

        Assert.Equal(MembershipType.Premium, customer.Memberships);
        Assert.True(customer.HasMembership(MembershipType.Premium));
    }

    [Fact]
    public void Activating_premium_grants_both_clubs()
    {
        var customer = TestData.Customer();

        customer.ActivateMembership(MembershipType.Premium);

        Assert.True(customer.HasMembership(MembershipType.BookClub));
        Assert.True(customer.HasMembership(MembershipType.VideoClub));
    }

    [Fact]
    public void Activating_a_membership_the_customer_already_has_reports_no_change()
    {
        var customer = TestData.Customer(memberships: MembershipType.BookClub);

        var change = customer.ActivateMembership(MembershipType.BookClub);

        Assert.False(change.Changed);
        Assert.Equal(MembershipType.BookClub, customer.Memberships);
    }

    [Fact]
    public void Activating_no_membership_is_rejected()
    {
        var customer = TestData.Customer();

        Assert.Throws<DomainRuleException>(() => customer.ActivateMembership(MembershipType.None));
    }
}
