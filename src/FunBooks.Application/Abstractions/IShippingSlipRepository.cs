using FunBooks.Domain.Shipping;

namespace FunBooks.Application.Abstractions;

public interface IShippingSlipRepository
{
    string NextId();

    Task AddAsync(ShippingSlip slip, CancellationToken cancellationToken);

    Task<ShippingSlip?> FindByOrderIdAsync(long orderId, CancellationToken cancellationToken);
}
