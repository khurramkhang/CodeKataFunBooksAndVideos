using FunBooks.Application.Customers;
using FunBooks.Application.Orders;
using FunBooks.Application.Processing;
using FunBooks.Application.Processing.Rules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FunBooks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.TryAddSingleton(TimeProvider.System);
        services.AddMetrics();

        
        services.TryAddEnumerable(ServiceDescriptor.Scoped<IPurchaseOrderRule, ActivateMembershipRule>());
        services.TryAddEnumerable(ServiceDescriptor.Scoped<IPurchaseOrderRule, GenerateShippingSlipRule>());

        services.TryAddScoped<IPurchaseOrderProcessor, PurchaseOrderProcessor>();
        services.TryAddScoped<IPurchaseOrderService, PurchaseOrderService>();
        services.TryAddScoped<ICustomerQueryService, CustomerQueryService>();

        return services;
    }
}
