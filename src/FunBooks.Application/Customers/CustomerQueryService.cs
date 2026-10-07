using System.Globalization;
using FunBooks.Application.Abstractions;
using FunBooks.Application.Common.Exceptions;
using FunBooks.Domain.Customers;

namespace FunBooks.Application.Customers;

public interface ICustomerQueryService
{
    Task<Customer> GetCustomerAsync(long customerId, CancellationToken cancellationToken);
}

public sealed class CustomerQueryService(ICurrentUser currentUser, ICustomerRepository customers) : ICustomerQueryService
{
    public async Task<Customer> GetCustomerAsync(long customerId, CancellationToken cancellationToken)
    {
        
        currentUser.EnsureCanAccessCustomer(customerId);

        return await customers.FindAsync(customerId, cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException("Customer", customerId.ToString(CultureInfo.InvariantCulture));
    }
}
