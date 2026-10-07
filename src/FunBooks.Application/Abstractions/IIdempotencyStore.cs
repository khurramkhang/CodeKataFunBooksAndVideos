namespace FunBooks.Application.Abstractions;
public readonly record struct IdempotentResult(long OrderId, bool Replayed);

public interface IIdempotencyStore
{
    Task<IdempotentResult> ExecuteAsync(
        string key,
        string requestFingerprint,
        Func<CancellationToken, Task<long>> operation,
        CancellationToken cancellationToken);
}
