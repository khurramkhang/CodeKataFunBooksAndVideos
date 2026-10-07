using FunBooks.Domain.Catalog;
using FunBooks.Domain.Common;
using FunBooks.Domain.Customers;

namespace FunBooks.Infrastructure.InMemory;
public static class SeedData
{
    public static IEnumerable<CatalogItem> Products() =>
    [
        new Book("BOOK-001", "Book 1", "Writer 1", BookFormat.Physical, new Money(8.50m)),
        new Book("BOOK-002", "Book 2", "Writer 2", BookFormat.Physical, new Money(32.99m)),
        new Book("BOOK-003", "Book 3", "Writer 3", BookFormat.EBook, new Money(19.99m)),
        new Video("VID-001", "Video 1", TimeSpan.FromMinutes(95), new Money(20.00m)),
        new Video("VID-002", "Video 2", TimeSpan.FromMinutes(60), new Money(15.00m)),
        new MembershipProduct("MEM-BOOK", "Book Club Membership", MembershipType.BookClub, new Money(20.00m)),
        new MembershipProduct("MEM-VIDEO", "Video Club Membership", MembershipType.VideoClub, new Money(20.00m)),
        new MembershipProduct("MEM-PREMIUM", "Premium Membership", MembershipType.Premium, new Money(35.00m)),
    ];

    public static IEnumerable<Customer> Customers() =>
    [
        new Customer(4567890, "Customer 1", "customer1@example.com", new Address("1 High Street", "Leeds", "LS17 5XX", "GB")),
        new Customer(1234567, "Customer 2", "customer2@example.com", new Address("2 High Street", "Leeds", "LS17 5XY", "GB"), MembershipType.VideoClub),
        new Customer(7654321, "Customer 3", "customer3@example.com", new Address("3 High Street", "Leeds", "LS17 5XZ", "GB"), MembershipType.Premium),
    ];
}
