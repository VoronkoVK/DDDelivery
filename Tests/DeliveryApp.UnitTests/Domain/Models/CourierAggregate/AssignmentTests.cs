using System;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Models.CourierAggregate;

public class AssignmentTests
{
    [Fact]
    public void Create_Valid_ShouldCreateAssignment()
    {
        var orderId = Guid.NewGuid();
        var volume = Volume.Create(10).Value;
        var location = Location.Create(1, 1).Value;
        var assignment = Assignment.Create(orderId, volume, location);
        
        assignment.Should().NotBeNull();
        assignment.OrderId.Should().Be(orderId);
        assignment.Volume.Should().Be(volume);
        assignment.Location.Should().Be(location);
        assignment.Status.Should().Be(Status.Assigned);
    }

    [Theory]
    [InlineData(4, 5)]
    [InlineData(5, 4)]
    [InlineData(5, 6)]
    [InlineData(6, 5)]
    public void Complete_ValidLocation_ShouldCompleteAssignment(int courierX, int courierY)
    {
        var assignment = Assignment.Create(Guid.NewGuid(), Volume.Create(10).Value, Location.Create(5, 5).Value);
        var courierLocation = Location.Create(courierX, courierY).Value;
        
        var result = assignment.Complete(courierLocation);

        result.IsSuccess.Should().BeTrue();
        assignment.Status.Should().Be(Status.Completed);
    }
    
    [Theory]
    [InlineData(3, 3)]
    [InlineData(8, 1)]
    [InlineData(5, 7)]
    public void Complete_InvalidLocation_ShouldFail(int courierX, int courierY)
    {
        var assignment = Assignment.Create(Guid.NewGuid(), Volume.Create(10).Value, Location.Create(5, 5).Value);
        var courierLocation = Location.Create(courierX, courierY).Value;
        
        var result = assignment.Complete(courierLocation);

        result.IsFailure.Should().BeTrue();
        assignment.Status.Should().Be(Status.Assigned);
    }

    [Fact]
    public void Complete_DoubleComplete_ShouldFail()
    {
        var assignment = Assignment.Create(Guid.NewGuid(), Volume.Create(10).Value, Location.Create(5, 5).Value);
        var courierLocation = Location.Create(5, 5).Value;
        
        var completedResult1 = assignment.Complete(courierLocation);
        var completedResult2 = assignment.Complete(courierLocation);

        assignment.Status.Should().Be(Status.Completed);
        completedResult1.IsSuccess.Should().BeTrue();
        completedResult2.IsFailure.Should().BeTrue();
    }
}