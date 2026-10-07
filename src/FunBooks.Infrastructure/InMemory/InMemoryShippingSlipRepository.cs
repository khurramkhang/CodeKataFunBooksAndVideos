using System.Collections.Concurrent;
using System.Globalization;
using FunBooks.Application.Abstractions;
using FunBooks.Domain.Shipping;

namespace FunBooks.Infrastructure.InMemory;

public sealed class InMemoryShippingSlipRepository : IShippingSlipRepository
{
    public Task AddAsync(ShippingSlip slip, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ShippingSlip?> FindByOrderIdAsync(long orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public string NextId()
    {
        throw new NotImplementedException();
    }
}
