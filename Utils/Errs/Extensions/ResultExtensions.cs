using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;

namespace Errs.Extensions;

public static class ResultExtensions
{
    /// <summary>
    /// Превращает Failure Result в исключение
    /// и возвращает значение при успехе.
    /// </summary>
    [DoesNotReturn]
    public static T GetValueOrThrow<T>(this Result<T, Error> result)
    {
        if (result.IsFailure)
            throw new DomainInvariantException(result.Error);

        return result.Value;
    }
}