using System.Collections.Concurrent;
using FunBooks.Application.Abstractions;
using FunBooks.Domain.Orders;

namespace FunBooks.Infrastructure.InMemory;

public sealed class InMemoryPurchaseOrderRepository : IPurchaseOrderRepository
{
    public Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(PurchaseOrder order, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<PurchaseOrder?> FindAsync(long orderId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public long NextId()
    {
        throw new NotImplementedException();
    } 
    public long PreviousId() { throw new NotImplementedException(); }
}
