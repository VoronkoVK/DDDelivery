using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.Models.SharedKernel;

public class Location : ValueObject
{
    public int X { get; }
    public int Y { get; }

    public Location()
    {
    }

    private Location(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public static Result<Location, Error> Create(int x, int y)
    {
        if (x is < 0 or > 10)
            return GeneralErrors.ValueMustBeBetween(nameof(x), x, 0, 10);
        
        if (y is < 0 or > 10)
            return GeneralErrors.ValueMustBeBetween(nameof(y), y, 0, 10);

        return new Location(x, y);
    }

    public static Location CreateRandom()
    {
        return new Location(Random.Shared.Next(0, 10), Random.Shared.Next(0, 11));
    }

    public Result<int, Error> DistanceTo(Location target)
    {
        if (target is null)
            return GeneralErrors.ValueIsRequired(nameof(target));
        
        var x = Math.Abs(target.X - X);
        var y = Math.Abs(target.Y - Y);

        return x + y;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}