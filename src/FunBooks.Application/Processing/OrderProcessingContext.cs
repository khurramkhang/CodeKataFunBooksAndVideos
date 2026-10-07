using FunBooks.Domain.Customers;
using FunBooks.Domain.Orders;
using FunBooks.Domain.Shipping;

namespace FunBooks.Application.Processing;






public sealed class OrderProcessingContext
{
    public OrderProcessingContext(PurchaseOrder order, Customer customer)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(customer);

        if (order.CustomerId != customer.Id)
        {
            throw new ArgumentException("The order does not belong to this customer.", nameof(customer));
        }

        Order = order;
        Customer = customer;
    }

    public PurchaseOrder Order { get; }

    public Customer Customer { get; }

    public ShippingSlip? ShippingSlip { get; set; }
}


public sealed record OrderProcessingResult(
    bool Succeeded,
    IReadOnlyList<RuleOutcome> Outcomes,
    ShippingSlip? ShippingSlip);
