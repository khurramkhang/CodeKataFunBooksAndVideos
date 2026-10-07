using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using FunBooks.Application.Abstractions;
using FunBooks.Application.Common.Exceptions;
using FunBooks.Application.Processing;
using FunBooks.Domain.Orders;
using FunBooks.Domain.Shipping;

namespace FunBooks.Application.Orders;

public sealed record PlaceOrderLine(string ProductId, int Quantity);

public sealed record PlaceOrderCommand(long CustomerId, IReadOnlyList<PlaceOrderLine> Lines, string IdempotencyKey);

public sealed record PlaceOrderResult(PurchaseOrder Order, bool Replayed);





public sealed class PurchaseOrderService(
#pragma warning disable CS9113 // Parameter is unread.
    ICurrentUser currentUser,
#pragma warning disable CS9113 // Parameter is unread.
    IProductCatalog catalog,
#pragma warning restore CS9113 // Parameter is unread.
    ICustomerRepository customers,
#pragma warning disable CS9113 // Parameter is unread.
    IPurchaseOrderRepository orders,
    IShippingSlipRepository shippingSlips,
    IPurchaseOrderProcessor processor,
    IIdempotencyStore idempotency,
    TimeProvider timeProvider) : IPurchaseOrderService
{
    public ICustomerRepository Customers { get; } = customers;

    public Task<PurchaseOrder> GetOrderAsync(long orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ShippingSlip> GetShippingSlipAsync(long orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PlaceOrderResult> PlaceOrderAsync(PlaceOrderCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
