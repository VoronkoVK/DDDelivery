using DeliveryApp.Core.Domain.Models.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Models.SharedKernel;

public class LocationTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 5)]
    [InlineData(10, 10)]
    public void Create_ValidValue_ShouldCreateLocation(int x, int y)
    {
        //  Arrange
        
        //  Act
        var location = Location.Create(x, y);
        
        //  Assert
        location.IsSuccess.Should().BeTrue();
        location.Value.X.Should().Be(x);
        location.Value.Y.Should().Be(y);
    }
    
    [Theory]
    [InlineData(-3, -3)]
    [InlineData(-5, 5)]
    [InlineData(11, 11)]
    public void Create_InvalidValue_ShouldFail(int x, int y)
    {
        //  Arrange
        
        //  Act
        var location = Location.Create(x, y);
        
        //  Assert
        location.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void CreateRandom_ShouldCreateRandomLocation()
    {
        //  Arrange
        
        //  Act
        var location = Location.CreateRandom();
        
        //  Assert
        location.Should().NotBeNull();
        location.X.Should().BeInRange(0, 10);
        location.Y.Should().BeInRange(0, 10);
    }
    
    [Fact]
    public void DistanceTo_WhenTargetIsNull_ShouldFail()
    {
        //  Arrange
        
        //  Act
        var location = Location.CreateRandom();
        var distance = location.DistanceTo(null);
        
        //  Assert
        distance.IsFailure.Should().BeTrue();
    }
    
    [Fact]
    public void DistanceTo_WithSameLocation_ShouldReturnZero()
    {
        //  Arrange
        
        //  Act
        var location1 = Location.Create(4, 5).Value;
        var location2 = Location.Create(4, 5).Value;
        var distance = location1.DistanceTo(location2);
        
        //  Assert
        distance.IsSuccess.Should().BeTrue();
        distance.Value.Should().Be(0);
    }

    [Fact]
    public void DistanceTo_WithDifferentLocation_ShouldReturnDistance()
    {
        //  Arrange
        
        //  Act
        var location1 = Location.Create(2, 7).Value;
        var location2 = Location.Create(8, 4).Value;
        var distance = location1.DistanceTo(location2);
        
        //  Assert
        distance.IsSuccess.Should().BeTrue();
        distance.Value.Should().Be(9);
    }
}