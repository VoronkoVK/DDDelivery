using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.Models.SharedKernel;

public class Volume : ValueObject
{
    public double Value { get; }

    private Volume()
    {
    }

    private Volume(double value)
    {
        Value = value;
    }

    public static Result<Volume, Error> Create(double value)
    {
        if (value <= 0)
            return GeneralErrors.ValueMustBeGreaterThan(nameof(value), value, 0);

        return new Volume(value);
    }
    
    public static Result<Volume, Error> Create(Volume volume)
    {
        if (volume is null)
            return GeneralErrors.ValueIsRequired(nameof(volume));

        return Create(volume.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}