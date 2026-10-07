using FunBooks.Domain.Customers;

namespace FunBooks.Application.Abstractions;

public interface ICustomerRepository
{
    Task<Customer?> FindAsync(long customerId, CancellationToken cancellationToken);

    Task SaveAsync(Customer customer, CancellationToken cancellationToken);
}
