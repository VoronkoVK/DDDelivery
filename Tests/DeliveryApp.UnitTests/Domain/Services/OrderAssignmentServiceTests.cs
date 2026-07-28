using System;
using System.Collections.Generic;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Domain.Models.OrderAggregate;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using DeliveryApp.Core.Domain.Services.OrderAssignment;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Services;

public class OrderAssignmentServiceTests
{
    private readonly OrderAssignmentService _service = new();

    [Fact]
    public void AssignOrder_EmptyCouriersList_ShouldFail()
    {
        var order = CreateOrder();

        var result = _service.AssignOrder(order, new List<Courier>());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderAssignmentService.Errors.CouriersListIsEmpty);
        order.Status.Should().Be(OrderStatus.Created);
    }

    [Fact]
    public void AssignOrder_NoSuitableCourier_ShouldFail()
    {
        var order = CreateOrder(volume: 21);
        var couriers = new List<Courier>
        {
            CreateCourier("Ivan", 1, 1),
            CreateCourier("Petr", 5, 5)
        };

        var result = _service.AssignOrder(order, couriers);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderAssignmentService.Errors.SuitableCourierNotFound);
        order.Status.Should().Be(OrderStatus.Created);
        couriers.Should().OnlyContain(courier => courier.Assignments.Count == 0);
    }

    [Fact]
    public void AssignOrder_AlreadyAssignedOrder_ShouldFail()
    {
        var order = CreateOrder();
        var assignedCourier = CreateCourier("Ivan", 1, 1);
        order.Assign(assignedCourier);
        var anotherCourier = CreateCourier("Petr", 5, 5);

        var result = _service.AssignOrder(order, new List<Courier> { anotherCourier });

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Order.Errors.OrderAlreadyAssigned);
        order.CourierId.Should().Be(assignedCourier.Id);
        anotherCourier.Assignments.Should().BeEmpty();
    }

    [Fact]
    public void AssignOrder_AvailableCouriers_ShouldAssignNearestCourier()
    {
        var order = CreateOrder(x: 5, y: 5);
        var distantCourier = CreateCourier("Ivan", 1, 1);
        var nearestCourier = CreateCourier("Petr", 4, 5);

        var result = _service.AssignOrder(
            order,
            new List<Courier> { distantCourier, nearestCourier });

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(nearestCourier);
        order.Status.Should().Be(OrderStatus.Assigned);
        order.CourierId.Should().Be(nearestCourier.Id);
        nearestCourier.Assignments.Should().ContainSingle();
        nearestCourier.Assignments[0].OrderId.Should().Be(order.Id);
        distantCourier.Assignments.Should().BeEmpty();
    }

    private static Order CreateOrder(int x = 5, int y = 5, double volume = 10)
    {
        var location = Location.Create(x, y).Value;
        var orderVolume = Volume.Create(volume).Value;
        return Order.Create(Guid.NewGuid(), location, orderVolume).Value;
    }

    private static Courier CreateCourier(string name, int x, int y)
    {
        var location = Location.Create(x, y).Value;
        return Courier.Create(name, location).Value;
    }
}
