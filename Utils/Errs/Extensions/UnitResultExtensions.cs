using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;

namespace Errs.Extensions;

public static class UnitResultExtensions
{
    /// <summary>
    /// Превращает UnitResult.Failure в исключение (fail-fast).
    /// Используется в местах, где ошибка невозможна по контракту.
    /// </summary>
    [DoesNotReturn]
    public static void ThrowIfFailure(this UnitResult<Error> result)
    {
        if (result.IsFailure)
            throw new DomainInvariantException(result.Error);
    }
}