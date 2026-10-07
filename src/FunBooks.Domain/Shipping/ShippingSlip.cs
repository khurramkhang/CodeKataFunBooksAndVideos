using FunBooks.Domain.Customers;
using FunBooks.Domain.Orders;
namespace FunBooks.Domain.Shipping;

public sealed record ShippingSlipItem(string ProductId, string Name, int Quantity);


public sealed class ShippingSlip
{
    public ShippingSlip(
        string id,
        long orderId,
        long customerId,
        string recipientName,
        Address deliveryAddress,
        IEnumerable<ShippingSlipItem> items,
        DateTimeOffset createdAt)
    {
        //ToDo: Validations
        var itemList = items.ToList();
        if (itemList.Count == 0)
        {
            throw new ArgumentException("A shipping slip needs at least one item.", nameof(items));
        }

        Id = id;
        OrderId = orderId;
        CustomerId = customerId;
        RecipientName = recipientName;
        DeliveryAddress = deliveryAddress;
        Items = itemList.AsReadOnly();
        CreatedAt = createdAt;
    }

    public string Id { get; }

    public long OrderId { get; }

    public long CustomerId { get; }

    public string RecipientName { get; }

    public Address DeliveryAddress { get; }

    public IReadOnlyList<ShippingSlipItem> Items { get; }

    public DateTimeOffset CreatedAt { get; }

    
    public static ShippingSlip CreateFor(string id, PurchaseOrder order, Customer customer, DateTimeOffset createdAt)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(customer);

        var items = order.PhysicalLines
            .Select(line => new ShippingSlipItem(line.Item.Id, line.Item.Name, line.Quantity));

        return new ShippingSlip(id, order.Id, customer.Id, customer.Name, customer.ShippingAddress, items, createdAt);
    }
}
