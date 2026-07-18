using System;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Domain.Models.OrderAggregate;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Models.CourierAggregate;

public class CourierTests
{
    [Fact]
    public void Create_ValidValues_ShouldCreateCourier()
    {
        var location = Location.Create(5, 5).Value;

        var courier = new Courier("Ivan", location);

        courier.Name.Should().Be("Ivan");
        courier.Location.Should().Be(location);
        courier.MaxVolume.Value.Should().Be(20);
        courier.Assignments.Should().BeEmpty();
    }

    [Fact]
    public void CanTakeOrder_WhenOrderIsNull_ShouldFail()
    {
        var courier = new Courier("Ivan", Location.Create(5, 5).Value);

        var result = courier.CanTakeOrder(null);

        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData(10, true)]
    [InlineData(20, true)]
    [InlineData(21, false)]
    public void CanTakeOrder_WithDifferentOrderVolume_ShouldReturnExpectedResult(int volume, bool expected)
    {
        var courier = new Courier("Ivan", Location.Create(5, 5).Value);
        var order = Order.Create(Location.Create(1, 1).Value, Volume.Create(volume).Value).Value;

        var result = courier.CanTakeOrder(order);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public void TakeOrder_ValidOrder_ShouldCreateAssignment()
    {
        var courier = new Courier("Ivan", Location.Create(5, 5).Value);
        var order = Order.Create(Location.Create(1, 1).Value, Volume.Create(10).Value).Value;

        var result = courier.TakeOrder(order);

        result.IsSuccess.Should().BeTrue();
        courier.Assignments.Should().ContainSingle();
        courier.Assignments[0].OrderId.Should().Be(order.Id);
    }

    [Theory]
    [InlineData(4, 5)]
    [InlineData(5, 4)]
    [InlineData(5, 6)]
    [InlineData(6, 5)]
    public void Move_ValidTarget_ShouldMoveCourier(int targetX, int targetY)
    {
        var courier = new Courier("Ivan", Location.Create(5, 5).Value);
        var target = Location.Create(targetX, targetY).Value;

        var result = courier.Move(target);

        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(target);
    }

    [Fact]
    public void Move_TargetIsCurrentLocation_ShouldFail()
    {
        var initialLocation = Location.Create(5, 5).Value;
        var courier = new Courier("Ivan", initialLocation);
        var target = Location.Create(5, 5).Value;

        var result = courier.Move(target);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(Courier.Errors.TargetLocationIsCurrent.Code);
        courier.Location.Should().Be(initialLocation);
    }

    [Fact]
    public void Move_TargetIsTooFar_ShouldFail()
    {
        var initialLocation = Location.Create(5, 5).Value;
        var courier = new Courier("Ivan", initialLocation);
        var target = Location.Create(7, 5).Value;

        var result = courier.Move(target);

        result.IsFailure.Should().BeTrue();
        courier.Location.Should().Be(initialLocation);
    }

    [Fact]
    public void CompleteOrder_CourierAtOrderLocation_ShouldCompleteAssignment()
    {
        var location = Location.Create(5, 5).Value;
        var courier = new Courier("Ivan", location);
        var order = Order.Create(location, Volume.Create(10).Value).Value;
        courier.TakeOrder(order);

        var result = courier.CompleteOrder(order);

        result.IsSuccess.Should().BeTrue();
        courier.Assignments[0].Status.Should().Be(Status.Completed);
    }

    [Fact]
    public void CompleteOrder_OrderIsNotAssigned_ShouldFail()
    {
        var courier = new Courier("Alex", Location.Create(5, 5).Value);
        var order = Order.Create(Location.Create(5, 5).Value, Volume.Create(10).Value).Value;

        var result = courier.CompleteOrder(order);

        result.IsFailure.Should().BeTrue();
    }
}
