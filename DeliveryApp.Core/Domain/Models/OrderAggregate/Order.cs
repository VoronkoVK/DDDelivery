using CSharpFunctionalExtensions;
using Ddd;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using Errs;

namespace DeliveryApp.Core.Domain.Models.OrderAggregate;

public class Order : Aggregate<Guid>
{
    public Location Location { get; private set; }
    public Volume Volume { get; private set; }
    public OrderStatus Status { get; private set; }
    public Guid? CourierId { get; private set; }

    public Order()
    {
    }

    private Order(Location location, Volume volume)
    {
        Id = Guid.NewGuid();
        Location = location;
        Volume = volume;
        Status = OrderStatus.Created;
    }

    public static Result<Order, Error> Create(Location location, Volume volume)
    {
        if (location is null)
            return GeneralErrors.ValueIsRequired(nameof(location));

        if (volume is null)
            return GeneralErrors.ValueIsRequired(nameof(volume));
        
        return new Order(location, volume);
    }

    public UnitResult<Error> Assign(Courier courier)
    {
        if (Status != OrderStatus.Created)
        {
            return Errors.OrderAlreadyAssigned;
        }
        
        CourierId = courier.Id;
        Status = OrderStatus.Assigned;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Complete()
    {
        if (Status == OrderStatus.Completed)
        {
            return Errors.OrderAlreadyCompleted;
        }
        
        if (Status != OrderStatus.Assigned)
        {
            return Errors.OrderIsNotAssigned;
        }

        Status = OrderStatus.Completed;
        return UnitResult.Success<Error>();
    }
    
    public static class Errors
    {
        public static Error OrderAlreadyCompleted =>
            new Error($"{nameof(Order).ToLowerInvariant()}.already.completed", "Order already completed");
        
        public static Error OrderAlreadyAssigned =>
            new Error($"{nameof(Order).ToLowerInvariant()}.already.assigned", "Order already assigned");
        
        public static Error OrderIsNotAssigned =>
            new Error($"{nameof(Order).ToLowerInvariant()}.is.not.assigned", "Order is not assigned");
    }
}