using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Domain.Models.OrderAggregate;
using Errs;

namespace DeliveryApp.Core.Domain.Services.OrderAssignment;

public interface IOrderAssignmentService
{
    Result<Courier, Error> AssignOrder(Order order, List<Courier> couriers);
}

public class OrderAssignmentService : IOrderAssignmentService
{
    public Result<Courier, Error> AssignOrder(Order order, List<Courier> couriers)
    {
        if (order.Status != OrderStatus.Created)
        {
            return Order.Errors.OrderAlreadyAssigned;
        }
        
        if (couriers.Count == 0)
        {
            return Errors.CouriersListIsEmpty;
        }
        
        var availableCouriers = couriers
            .Where(c =>
            {
                var canTakeOrder = c.CanTakeOrder(order);
                return canTakeOrder.IsSuccess && canTakeOrder.Value;
            })
            .ToList();
        
        if (availableCouriers.Count == 0)
        {
            return Errors.SuitableCourierNotFound;
        }
        
        var courier = availableCouriers
            .OrderBy(c => c.Location.DistanceTo(order.Location).Value)
            .First();

        var takeResult = courier.TakeOrder(order);
        if (takeResult.IsFailure)
        {
            return takeResult.Error;
        }

        var assignResult = order.Assign(courier);
        if (assignResult.IsFailure)
        {
            return assignResult.Error;
        }

        return courier;
    }

    public class Errors
    {
        public static Error CouriersListIsEmpty =>
            new Error("couriers.list.is.empty", "Couriers list is empty");

        public static Error SuitableCourierNotFound =>
            new Error("suitable.courier.not.found", "Suitable courier not found");

    }
}
