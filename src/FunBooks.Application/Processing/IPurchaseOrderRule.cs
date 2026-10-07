using FunBooks.Domain.Orders;

namespace FunBooks.Application.Processing;






public interface IPurchaseOrderRule
{
    
    string Name { get; }

    
    string Description { get; }

    
    int Order { get; }

    
    bool AppliesTo(OrderProcessingContext context);

    
    Task<RuleOutcome> ApplyAsync(OrderProcessingContext context, CancellationToken cancellationToken);
}
