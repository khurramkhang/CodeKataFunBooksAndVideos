using FunBooks.Domain.Orders;

namespace FunBooks.Application.Abstractions;

public interface IPurchaseOrderRepository
{
    long NextId();

    Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken);

    Task UpdateAsync(PurchaseOrder order, CancellationToken cancellationToken);

    Task<PurchaseOrder?> FindAsync(long orderId, CancellationToken cancellationToken);
}
