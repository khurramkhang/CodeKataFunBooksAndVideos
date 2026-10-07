namespace FunBooks.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string resource, string key)
        : base($"{resource} '{key}' was not found.")
    {
        Resource = resource;
        Key = key;
    }

    public string Resource { get; }

    public string Key { get; }
}


public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException()
        : base("You are not allowed to perform this action.")
    {
    }
}


public sealed class IdempotencyConflictException : Exception
{
    public IdempotencyConflictException()
        : base("This Idempotency-Key was already used with a different request. Use a new key for a new order.")
    {
    }
}
