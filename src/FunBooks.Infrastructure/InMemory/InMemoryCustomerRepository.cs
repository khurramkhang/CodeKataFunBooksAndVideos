using System.Collections.Concurrent;
using FunBooks.Application.Abstractions;
using FunBooks.Domain.Customers;

namespace FunBooks.Infrastructure.InMemory;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly ConcurrentDictionary<long, Customer> _customers;

    public InMemoryCustomerRepository(IEnumerable<Customer> customers)
    {
        ArgumentNullException.ThrowIfNull(customers);
        _customers = new ConcurrentDictionary<long, Customer>(customers.ToDictionary(customer => customer.Id));
    }

    public Task<Customer?> FindAsync(long customerId, CancellationToken cancellationToken) =>
        Task.FromResult(_customers.GetValueOrDefault(customerId));

    public Task SaveAsync(Customer customer, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(customer);
        _customers[customer.Id] = customer;
        return Task.CompletedTask;
    }
}
