using FunBooks.Domain.Orders;
using FunBooks.Domain.Shipping;

namespace FunBooks.Application.Orders;

public interface IPurchaseOrderService
{
    Task<PlaceOrderResult> PlaceOrderAsync(PlaceOrderCommand command, CancellationToken cancellationToken);

    Task<PurchaseOrder> GetOrderAsync(long orderId, CancellationToken cancellationToken);

    Task<ShippingSlip> GetShippingSlipAsync(long orderId, CancellationToken cancellationToken);
}
