using BestRoute.Models;
using BestRoute.Services;

namespace BestRoute.UnitTests.Services;

public class DijkstraRouteAlgorithmServiceTest
{
    [Theory]
    [MemberData(nameof(RouteTestDataGenerator.GetRouteTestData), MemberType = typeof(RouteTestDataGenerator))]
    public void CalculateRoute_WhenExistBestRoute_ShouldReturnBestRoute(RouteTestData testData)
    {
        // Arrange
        var service = new DijkstraRouteAlgorithmService();

        // Act
        var result = service.CalculateRoute(testData.Origin, testData.Destination, testData.Routes);

        // Assert
        Assert.Equal(testData.ExpectedRoute, result.ListTerminals());
        Assert.Equal(testData.ExpectedValue, result.GetValue());
    }

    [Fact]
    public void CalculateRoute_WhenNotExistBestRoute_ShouldThrowInvalidRouteException()
    {
        // Arrange
        var origin = "GRU";
        var destination = "CDG";
        List<SingleRoute> routesCase1 =
            [
                new ("GRU", "BRC", 10),
                new ("SCL", "ORL", 10),
                new ("ORL", "CDG", 10),
            ];
        List<SingleRoute> routesCase2 =
            [
                new ("ORL", "BRC", 10),
                new ("SCL", "ORL", 10),
                new ("ORL", "SCL", 10),
            ];
        var service = new DijkstraRouteAlgorithmService();

        // Act
        var result1 = Record.Exception(() => service.CalculateRoute(origin, destination, routesCase1));
        var result2 = Record.Exception(() => service.CalculateRoute(origin, destination, routesCase2));

        // Assert
        Assert.IsType<InvalidRouteException>(result1);
        Assert.IsType<InvalidRouteException>(result2);
    }
}
