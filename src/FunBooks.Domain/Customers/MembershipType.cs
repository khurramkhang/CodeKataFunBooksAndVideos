namespace FunBooks.Domain.Customers;





[Flags]
#pragma warning disable CA1711 
public enum MembershipType
#pragma warning restore CA1711
{
    None = 0,
    BookClub = 1,
    VideoClub = 2,
    Premium = BookClub | VideoClub,
}
