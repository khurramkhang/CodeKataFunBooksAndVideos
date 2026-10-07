using FunBooks.Domain.Catalog;
using FunBooks.Domain.Common;
using FunBooks.Domain.Customers;
using FunBooks.Domain.Orders;

namespace FunBooks.UnitTests.TestSupport;
internal static class TestData
{
    public static readonly DateTimeOffset Now = new(2026, 10, 7, 11, 30, 0, TimeSpan.Zero);

    public static Video FirstAidVideo() =>
        new("VID-001", "Video 1", TimeSpan.FromMinutes(95), new Money(20.00m));

    public static Book GirlOnTheTrain() =>
        new("BOOK-001", "Book 1", "Paula Hawkins", BookFormat.Physical, new Money(8.50m));

    public static Book CleanCode() =>
        new("BOOK-002", "Book 2", "Robert C. Martin", BookFormat.Physical, new Money(32.99m));

    public static Book EBook() =>
        new("BOOK-003", "Book 3", "Vaughn Vernon", BookFormat.EBook, new Money(19.99m));

    public static MembershipProduct BookClub() =>
        new("MEM-BOOK", "Book Club Membership", MembershipType.BookClub, new Money(20.00m));

    public static MembershipProduct VideoClub() =>
        new("MEM-VIDEO", "Video Club Membership", MembershipType.VideoClub, new Money(20.00m));

    public static Customer Customer(long id = 4567890, MembershipType memberships = MembershipType.None) =>
        new(id, "Customer 1", "customer1@example.com", new Address("1 High Street", "Leeds", "LS17 5XX", "GB"), memberships);

    public static PurchaseOrder Order(long customerId, params CatalogItem[] items) =>
        PurchaseOrder.Create(3344656, customerId, items.Select(item => new OrderLine(item, 1)), Now);

    /// <summary>The example order from the brief: Video + Book + Book Club = 48.50.</summary>
    public static PurchaseOrder ExampleOrder(long customerId = 4567890) =>
        Order(customerId, FirstAidVideo(), GirlOnTheTrain(), BookClub());
}
