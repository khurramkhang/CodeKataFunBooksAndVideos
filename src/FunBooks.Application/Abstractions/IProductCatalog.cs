using FunBooks.Domain.Catalog;

namespace FunBooks.Application.Abstractions;

public interface IProductCatalog
{
    Task<CatalogItem?> FindAsync(string productId, CancellationToken cancellationToken);

    Task<IReadOnlyList<CatalogItem>> GetAllAsync(CancellationToken cancellationToken);
}
