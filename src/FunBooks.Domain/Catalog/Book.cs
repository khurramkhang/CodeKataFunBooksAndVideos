using FunBooks.Domain.Common;

namespace FunBooks.Domain.Catalog;

public enum BookFormat
{
    Physical,
    EBook,
}


public sealed class Book : CatalogItem
{
    public Book(string id, string name, string author, BookFormat format, Money price)
        : base(id, name, price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(author);
        Author = author;
        Format = format;
    }

    public string Author { get; }

    public BookFormat Format { get; }

    public override bool IsPhysical => Format == BookFormat.Physical;

    public override ProductKind Kind => ProductKind.Book;
}
