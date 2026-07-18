using CSharpFunctionalExtensions;
using Ddd;
using DeliveryApp.Core.Domain.Models.OrderAggregate;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using Errs;

namespace DeliveryApp.Core.Domain.Models.CourierAggregate;

public class Courier : Aggregate<Guid>
{
    public string Name { get; private set; }
    public Location Location { get; private set; }
    public Volume MaxVolume { get; private set; }
    public IReadOnlyList<Assignment> Assignments => _assignments;
    
    private List<Assignment> _assignments = new List<Assignment>();

    public Courier()
    {
    }

    public Courier(string name, Location location)
    {
        Name = name;
        Location = location;
        MaxVolume = Volume.Create(20).Value;
    }
    
    public Result<bool, Error> CanTakeOrder(Order order)
    {
        if (order == null)
            return GeneralErrors.ValueIsRequired(nameof(order));
        
        var currentVolume = _assignments.Sum(a => a.Volume.Value);
        return currentVolume + order.Volume.Value <= MaxVolume.Value;
    }

    public UnitResult<Error> TakeOrder(Order order)
    {
        var canTake = CanTakeOrder(order);
        if (canTake.IsFailure)
            return canTake.Error;

        var volume = Volume.Create(order.Volume.Value).Value;
        var location = Location.Create(order.Location).Value;
        var assignment = Assignment.Create(order.Id, volume, location);
        if (assignment.IsFailure)
            return assignment.Error;
        
        _assignments.Add(assignment.Value);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> CompleteOrder(Order order)
    {
        if (order == null)
            return GeneralErrors.ValueIsRequired(nameof(order));
        
        var assignment = _assignments.FirstOrDefault(a => a.OrderId == order.Id);
        if (assignment == null)
            return Errors.AssignmentNotFound;

        var result = assignment.Complete(Location);
        if (result.IsFailure)
            return result.Error;
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> Move(Location target)
    {
        if (target == null)
            return GeneralErrors.ValueIsRequired(nameof(target));

        if (target == Location)
        {
            return Errors.TargetLocationIsCurrent;
        }

        var distance = Location.DistanceTo(target);
        if (distance.IsFailure)
        {
            return Errors.TargetInvalidDistance;
        } else if (distance.Value > 1)
        {
            return Errors.TargetDistanceTooLong;
        }
        
        Location = Location.Create(target).Value;

        return UnitResult.Success<Error>();
    }

    public class Errors
    {
        public static Error AssignmentNotFound =>
            new Error($"{nameof(Assignment).ToLowerInvariant()}.not.found", "Assignment not found");
        
        public static Error TargetInvalidDistance =>
            new Error("target.invalid.distance", "Target invalid distance");

        public static Error TargetLocationIsCurrent =>
            new Error("target.location.is.current", "Target location is current location");
        
        public static Error TargetDistanceTooLong =>
            new Error("target.distance.too.long", "Target distance is too long");
    }
}
