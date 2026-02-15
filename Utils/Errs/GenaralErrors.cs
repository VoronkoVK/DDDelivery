using System.Numerics;

namespace Errs;

public static class GeneralErrors
{
    public static Error NotFound<TId>(string name, TId id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        return new Error(
            "record.not.found",
            $"Record not found. Name: {name}, id: {id}");
    }

    public static Error ValueIsRequired(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        return new Error(
            "value.must.be.provided",
            $"Value must be provided for {name}.");
    }

    public static Error ValueIsInvalid<T>(string name, T value)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        return new Error(
            "value.is.invalid",
            $"Value '{value}' is invalid for {name}.");
    }

    public static Error InvalidLength(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        return new Error(
            "invalid.string.length",
            $"Invalid {name} length.");
    }

    public static Error CollectionIsTooSmall(int min, int current)
    {
        return new Error(
            "collection.must.contain.at.least",
            $"The collection must contain at least {min} items. It contains {current}.");
    }

    public static Error CollectionIsTooLarge(int max, int current)
    {
        return new Error(
            "collection.must.contain.at.most",
            $"The collection must contain at most {max} items. It contains {current}.");
    }

    public static Error ValueMustBeBetween<T>(
        string name,
        T value,
        T min,
        T max)
        where T : INumber<T>
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        return new Error(
            "value.must.be.between",
            $"The value of {name} ({value}) must be between {min} and {max}.");
    }

    public static Error ValueMustBeGreaterThan<T>(
        string name,
        T value,
        T min)
        where T : INumber<T>
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        return new Error(
            "value.must.be.greater.than",
            $"The value of {name} ({value}) must be greater than {min}.");
    }

    public static Error ValueMustBeGreaterOrEqual<T>(
        string name,
        T value,
        T min)
        where T : INumber<T>
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        return new Error(
            "value.must.be.greater.or.equal",
            $"The value of {name} ({value}) must be greater than or equal to {min}.");
    }

    public static Error ValueMustBeLessThan<T>(
        string name,
        T value,
        T max)
        where T : INumber<T>
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        return new Error(
            "value.must.be.less.than",
            $"The value of {name} ({value}) must be less than {max}.");
    }

    public static Error ValueMustBeLessOrEqual<T>(
        string name,
        T value,
        T max)
        where T : INumber<T>
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        return new Error(
            "value.must.be.less.or.equal",
            $"The value of {name} ({value}) must be less than or equal to {max}.");
    }
}