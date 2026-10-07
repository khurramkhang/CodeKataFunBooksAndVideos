using FunBooks.Application.Common.Exceptions;

namespace FunBooks.Application.Abstractions;


public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    
    string UserId { get; }

    
    long? CustomerId { get; }

    bool IsAdmin { get; }
}

public static class CurrentUserExtensions
{
    
    public static bool CanAccessCustomer(this ICurrentUser user, long customerId)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.IsAuthenticated && (user.IsAdmin || user.CustomerId == customerId);
    }

    public static void EnsureCanAccessCustomer(this ICurrentUser user, long customerId)
    {
        if (!user.CanAccessCustomer(customerId))
        {
            throw new ForbiddenAccessException();
        }
    }
}
