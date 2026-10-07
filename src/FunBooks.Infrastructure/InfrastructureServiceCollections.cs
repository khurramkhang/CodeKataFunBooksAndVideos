using FunBooks.Application.Abstractions;
using FunBooks.Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FunBooks.Infrastructure;

public static class InfrastructureServiceCollections
{
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);
        //Using InMemory implementations for the purpose of this sample application.        
        services.TryAddSingleton<IProductCatalog>(_ => new InMemoryProductCatalog(SeedData.Products()));
        services.TryAddSingleton<ICustomerRepository>(_ => new InMemoryCustomerRepository(SeedData.Customers()));
        services.TryAddSingleton<IPurchaseOrderRepository, InMemoryPurchaseOrderRepository>();
        services.TryAddSingleton<IShippingSlipRepository, InMemoryShippingSlipRepository>();
        services.TryAddSingleton<IIdempotencyStore, InMemoryIdempotencyStore>();
        return services;
    }
}
