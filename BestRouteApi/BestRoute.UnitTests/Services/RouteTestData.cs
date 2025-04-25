using BestRoute.Models;

namespace BestRoute.UnitTests.Services;

public class RouteTestData
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public List<SingleRoute> Routes { get; set; }
    public List<string> ExpectedRoute { get; set; }
    public int ExpectedValue { get; set; }

    public RouteTestData(string origin, string destination, List<SingleRoute> routes, List<string> expectedRoute, int expectedValue)
    {
        Origin = origin;
        Destination = destination;
        Routes = routes;
        ExpectedRoute = expectedRoute;
        ExpectedValue = expectedValue;
    }
}