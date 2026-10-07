using FunBooks.Application.Abstractions;

namespace FunBooks.Infrastructure.InMemory;

public sealed class InMemoryIdempotencyStore(TimeProvider timeProvider) : IIdempotencyStore
{
    
    public Task<IdempotentResult> ExecuteAsync(string key, string requestFingerprint, Func<CancellationToken, Task<long>> operation, CancellationToken cancellationToken)
    {
        //ToDo: Dummy implementation for now
        return Task.Run(async () =>
        {
            Random random = new Random();
            var now = timeProvider.GetUtcNow();
            return new IdempotentResult
            {
                OrderId = random.Next(1, int.MaxValue),
                Replayed = false
            };
        });
    }
}
