namespace FunBooks.Domain.Customers;


public sealed record Address
{
    public Address(string line1, string city, string postcode, string country)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(line1);
        //ToDo:Validations

        Line1 = line1;
        City = city;
        Postcode = postcode;
        Country = country;
    }

    public string Line1 { get; }

    public string City { get; }

    public string Postcode { get; }

    public string Country { get; }
}
