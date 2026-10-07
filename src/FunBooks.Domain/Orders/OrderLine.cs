using FunBooks.Domain.Catalog;
using FunBooks.Domain.Common;

namespace FunBooks.Domain.Orders;


public sealed class OrderLine
{
    public const int MaxQuantity = 100;

    public OrderLine(CatalogItem item, int quantity)
    {
        //ToDo: Validations

        Item = item;
        Quantity = quantity;
        LineTotal = item.Price.Multiply(quantity);
    }

    public CatalogItem Item { get; }

    public int Quantity { get; }

    public Money LineTotal { get; }
}
