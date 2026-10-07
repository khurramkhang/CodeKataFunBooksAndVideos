using FunBooks.Domain.Common;

namespace FunBooks.Domain.Catalog;

public enum ProductKind
{
    Book,
    Video,
    Membership,
}


public abstract class CatalogItem
{
    protected CatalogItem(string id, string name, Money price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Id = id;
        Name = name;
        Price = price;
    }

    public string Id { get; }

    public string Name { get; }

    public Money Price { get; }

    
    public abstract bool IsPhysical { get; }

    public abstract ProductKind Kind { get; }
}
