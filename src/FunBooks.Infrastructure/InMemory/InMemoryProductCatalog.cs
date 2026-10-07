using System.Collections.Frozen;
using FunBooks.Application.Abstractions;
using FunBooks.Domain.Catalog;

namespace FunBooks.Infrastructure.InMemory;

public sealed class InMemoryProductCatalog : IProductCatalog
{
    private readonly FrozenDictionary<string, CatalogItem> _items;
    private readonly IReadOnlyList<CatalogItem> _all;

    public InMemoryProductCatalog(IEnumerable<CatalogItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        _items = items.ToFrozenDictionary(item => item.Id, StringComparer.OrdinalIgnoreCase);
        _all = _items.Values.OrderBy(item => item.Id, StringComparer.Ordinal).ToArray();
    }

    public Task<CatalogItem?> FindAsync(string productId, CancellationToken cancellationToken) =>
        Task.FromResult(_items.GetValueOrDefault(productId));

    public Task<IReadOnlyList<CatalogItem>> GetAllAsync(CancellationToken cancellationToken) =>
        Task.FromResult(_all);
}
