namespace Errs.Extensions;

/// <summary>
/// Исключение для нарушения доменных инвариантов.
/// Используется как сигнал бага системы.
/// </summary>
public sealed class DomainInvariantException : InvalidOperationException
{
    public Error Error { get; }

    public DomainInvariantException(Error error)
        : base($"Domain invariant violated: {error.Code}. {error.Message}")
    {
        Error = error;
    }
}