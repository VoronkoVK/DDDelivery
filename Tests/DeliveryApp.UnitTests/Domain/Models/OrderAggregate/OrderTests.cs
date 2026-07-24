using System;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Domain.Models.OrderAggregate;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Models.OrderAggregate;

public class OrderTests
{
    [Fact]
    public void Create_ValidValues_ShouldCreateOrder()
    {
        var location = Location.Create(5, 5).Value;
        var volume = Volume.Create(10).Value;

        var result = Order.Create(Guid.NewGuid(), location, volume);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.Location.Should().Be(location);
        result.Value.Volume.Should().Be(volume);
        result.Value.Status.Should().Be(OrderStatus.Created);
        result.Value.CourierId.Should().BeNull();
    }

    [Fact]
    public void Create_ValuesIsNull_ShouldFail()
    {
        var result = Order.Create(Guid.Empty, null, null);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Assign_CreatedOrder_ShouldAssignCourier()
    {
        var order = Order.Create(Guid.NewGuid(), Location.Create(5, 5).Value, Volume.Create(10).Value).Value;
        var courier = Courier.Create("Ivan", Location.Create(1, 1).Value).Value;

        var result = order.Assign(courier);

        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Assigned);
        order.CourierId.Should().Be(courier.Id);
    }

    [Fact]
    public void Assign_AlreadyAssignedOrder_ShouldFail()
    {
        var order = Order.Create(Guid.NewGuid(), Location.Create(5, 5).Value, Volume.Create(10).Value).Value;
        var courier = Courier.Create("Ivan", Location.Create(1, 1).Value).Value;
        order.Assign(courier);

        var result = order.Assign(courier);

        result.IsFailure.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Assigned);
    }

    [Fact]
    public void Complete_CreatedOrder_ShouldFail()
    {
        var order = Order.Create(Guid.NewGuid(), Location.Create(5, 5).Value, Volume.Create(10).Value).Value;

        var result = order.Complete();

        result.IsFailure.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Created);
    }

    [Fact]
    public void Complete_AssignedOrder_ShouldCompleteOrder()
    {
        var order = Order.Create(Guid.NewGuid(), Location.Create(5, 5).Value, Volume.Create(10).Value).Value;
        var courier = Courier.Create("Ivan", Location.Create(1, 1).Value).Value;
        order.Assign(courier);

        var result = order.Complete();

        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public void Complete_AlreadyCompletedOrder_ShouldFail()
    {
        var order = Order.Create(Guid.NewGuid(), Location.Create(5, 5).Value, Volume.Create(10).Value).Value;
        var courier = Courier.Create("Ivan", Location.Create(1, 1).Value).Value;
        order.Assign(courier);
        order.Complete();

        var result = order.Complete();

        result.IsFailure.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Completed);
    }
}
