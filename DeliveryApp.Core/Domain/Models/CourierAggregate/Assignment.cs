using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using Errs;

namespace DeliveryApp.Core.Domain.Models.CourierAggregate;

public class Assignment : Entity<Guid>
{
    private Assignment()
    {
    }

    private Assignment(Guid orderId, Volume volume, Location location, Status status)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Volume = volume;
        Location = location;
        Status = status;
    }

    public Guid OrderId { get; private set; }
    public Volume Volume { get; private set; }
    public Location Location { get; private set; }
    public Status Status { get; private set; }

    public static Assignment Create(Guid orderId, Volume volume, Location location)
    {
        return new Assignment(orderId, volume, location, Status.Assigned);
    }

    public Result<object, Error> Complete(Location courierLocation)
    {
        if (Status == Status.Completed)
        {
            return Errors.AssignmentAlreadyCompleted;
        }

        var distance = Location.DistanceTo(courierLocation);
        if (distance.IsFailure || distance.Value > 1)
        {
            return Errors.AssignmentInvalidDistance;
        }

        Status = Status.Completed;
        return new object();
    }

    public static class Errors
    {
        public static Error AssignmentAlreadyCompleted =>
            new Error($"{nameof(Assignment).ToLowerInvariant()}.already.completed", "Assignment already completed");

        public static Error AssignmentInvalidDistance =>
            new Error($"{nameof(Assignment).ToLowerInvariant()}.invalid.distance", "Assignment invalid distance");
    }
}