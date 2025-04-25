using System;
using System.Collections.Generic;
using System.Linq;
using BestRoute.Models;
using Xunit;

namespace BestRoute.UnitTests.Models;

public class ConnectedRouteTest
{
    [Fact]
    public void Constructor_ShouldInitializeRoutes_WhenValidRoutesProvided()
    {
        // Arrange
        var singleRoutes = new List<SingleRoute>
        {
            new ("AAA", "BBB", 10),
            new ("BBB", "CCC", 20)
        };

        // Act
        var connectedRoute = new ConnectedRoute(singleRoutes);

        // Assert
        Assert.Equal("AAA", connectedRoute.Origin);
        Assert.Equal("CCC", connectedRoute.Destination);
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidRouteException_WhenLessThanTwoRoutes()
    {
        // Arrange
        var singleRoutes = new List<SingleRoute> { new ("AAA", "BBB", 10) };

        // Act & Assert
        Assert.Throws<InvalidRouteException>(() => new ConnectedRoute(singleRoutes));
    }

    [Fact]
    public void GetValue_ShouldReturnSumOfAllRoutesValues()
    {
        // Arrange
        var singleRoutes = new List<SingleRoute>
        {
            new ("AAA", "BBB", 10),
            new ("BBB", "CCC", 20)
        };
        var connectedRoute = new ConnectedRoute(singleRoutes);

        // Act
        var value = connectedRoute.GetValue();

        // Assert
        Assert.Equal(30, value);
    }

    [Fact]
    public void ListTerminals_ShouldReturnAllTerminalsInOrder()
    {
        // Arrange
        var singleRoutes = new List<SingleRoute>
        {
            new ("AAA", "BBB", 10),
            new ("BBB", "CCC", 20)
        };
        var connectedRoute = new ConnectedRoute(singleRoutes);

        // Act
        var terminals = connectedRoute.ListTerminals();

        // Assert
        Assert.Equal([ "AAA", "BBB", "CCC" ], terminals);
    }
}
