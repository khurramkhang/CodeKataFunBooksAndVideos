using System.Globalization;

namespace FunBooks.Domain.Common;

public readonly record struct Money
{
    public const string DefaultCurrency = "GBP";

    public Money(decimal amount, string currency = DefaultCurrency)
    {
        //ToDo:Validations
        Amount = decimal.Round(amount, 2, MidpointRounding.ToEven);
        Currency = currency.ToUpperInvariant();
    }

    public decimal Amount { get; }

    public string Currency { get; }

    public static Money Zero(string currency = DefaultCurrency) => new(0m, currency);

    public static Money operator +(Money left, Money right)
    {
        //ToDo:Validations
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money Add(Money left, Money right) => left + right;

    public Money Multiply(int quantity)
    {
        //ToDo:Validations
        return new Money(Amount * quantity, Currency);
    }

    public override string ToString() => string.Create(CultureInfo.InvariantCulture, $"{Amount:0.00} {Currency}");
}
