using System.Diagnostics.CodeAnalysis;

namespace Errs.Extensions;

public static class ErrorExtensions
{
    /// <summary>
    /// Бросает <see cref="DomainInvariantException"/> на основе доменной ошибки.
    /// Используется в местах, где ошибка невозможна по контракту.
    /// </summary>
    [DoesNotReturn]
    public static void ThrowException(this Error error)
    {
        throw new DomainInvariantException(error);
    }
}