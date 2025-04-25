namespace BestRoute.Services;

using BestRoute.Models;

public interface IRouteCalculatorService
{
    IRoute CalculateRoute(string origin, string destination, IEnumerable<SingleRoute> routes);
}
