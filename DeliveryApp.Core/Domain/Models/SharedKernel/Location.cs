using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.Models.SharedKernel;

public class Location : ValueObject
{
    public const int MinBoundary = 1;
    public const int MaxBoundary = 10;
    public int X { get; }
    public int Y { get; }

    private Location()
    {
    }

    private Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Result<Location, Error> Create(int x, int y)
    {
        if (x is < MinBoundary or > MaxBoundary)
            return GeneralErrors.ValueMustBeBetween(nameof(x), x, MinBoundary, MaxBoundary);

        if (y is < MinBoundary or > MaxBoundary)
            return GeneralErrors.ValueMustBeBetween(nameof(y), y, MinBoundary, MaxBoundary);

        return new Location(x, y);
    }

    public static Location CreateRandom()
    {
        return new Location(Random.Shared.Next(MinBoundary, MaxBoundary + 1),
            Random.Shared.Next(MinBoundary, MaxBoundary + 1));
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