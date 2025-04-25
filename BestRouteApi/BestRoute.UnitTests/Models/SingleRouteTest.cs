using System;
using System.Collections.Generic;
using BestRoute.Models;
using Xunit;

namespace BestRoute.UnitTests.Models;

public class SingleRouteTest
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var origin = "ABC";
        var destination = "DEF";
        var value = 100m;

        // Act
        var route = new SingleRoute(origin, destination, value);

        // Assert
        Assert.Equal(origin.ToUpper(), route.Origin);
        Assert.Equal(destination.ToUpper(), route.Destination);
        Assert.Equal(value, route.Value);
        Assert.NotEqual(Guid.Empty, route.Id);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenInvalidParameters()
    {
        // Arrange
        var invalidOrigin = "AB";
        var invalidDestination = "DE";
        var invalidValue = -10m;

        // Act & Assert
        Assert.Throws<InvalidRouteException>(() => new SingleRoute(invalidOrigin, "DEF", 100m));
        Assert.Throws<InvalidRouteException>(() => new SingleRoute("ABC", invalidDestination, 100m));
        Assert.Throws<InvalidRouteException>(() => new SingleRoute("ABC", "DEF", invalidValue));
    }

    [Fact]
    public void Update_ShouldUpdateProperties()
    {
        // Arrange
        var route = new SingleRoute("ABC", "DEF", 100m);
        var newOrigin = "GHI";
        var newDestination = "JKL";
        var newValue = 200m;

        // Act
        route.Update(newOrigin, newDestination, newValue);

        // Assert
        Assert.Equal(newOrigin.ToUpper(), route.Origin);
        Assert.Equal(newDestination.ToUpper(), route.Destination);
        Assert.Equal(newValue, route.Value);
    }

    [Fact]
    public void Update_ShouldThrowException_WhenInvalidParameters()
    {
        // Arrange
        var route = new SingleRoute("ABC", "DEF", 100m);
        var invalidOrigin = "GH";
        var invalidDestination = "JK";
        var invalidValue = -200m;

        // Act & Assert
        Assert.Throws<InvalidRouteException>(() => route.Update(invalidOrigin, "JKL", 200m));
        Assert.Throws<InvalidRouteException>(() => route.Update("GHI", invalidDestination, 200m));
        Assert.Throws<InvalidRouteException>(() => route.Update("GHI", "JKL", invalidValue));
    }

    [Fact]
    public void ListTerminals_ShouldReturnOriginAndDestination()
    {
        // Arrange
        var route = new SingleRoute("ABC", "DEF", 100m);

        // Act
        var terminals = route.ListTerminals();

        // Assert
        Assert.Equal(["ABC", "DEF"], terminals);
    }
}
