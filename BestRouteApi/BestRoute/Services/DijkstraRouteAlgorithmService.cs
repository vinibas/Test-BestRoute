
using BestRoute.Models;

namespace BestRoute.Services;

public class DijkstraRouteAlgorithmService : IRouteCalculatorService
{
    private record Edge
    {
        public SingleRoute Route { get; }
        public bool IsProcessed { get; set; } = false;
        public decimal Distance { get; set; } = decimal.MaxValue;
        public SingleRoute? PreviousRoute { get; set; } = null;

        public Edge(SingleRoute route) => Route = route;
    }

    public IRoute CalculateRoute(string origin, string destination, IEnumerable<SingleRoute> routes)
    {
        var edges = routes.Select(route => new Edge(route)).ToList();
        var graph = edges.GroupBy(edge => edge.Route.Origin).ToDictionary(group => group.Key, group => group.ToList());

        var thereIsNoRouteWithTheOrigin = !graph.ContainsKey(origin);
        var thereIsNoRouteToTheDestination = !edges.Exists(e => e.Route.Destination == destination);
        if (thereIsNoRouteWithTheOrigin || thereIsNoRouteToTheDestination)
            throw new InvalidRouteException([ "Rota inexistente." ]);

        var startEdge = edges.First(edge => edge.Route.Origin == origin);
        startEdge.Distance = 0;

        var queue = new Queue<Edge>();
        queue.Enqueue(startEdge);

        while (queue.Count > 0)
        {
            var currentEdge = queue.Dequeue();
            currentEdge.IsProcessed = true;

            var neighbors = graph.GetValueOrDefault(currentEdge.Route.Destination);
            if (neighbors is null) continue;

            foreach (var neighbor in neighbors)
            {
                if (neighbor.IsProcessed) continue;

                var newDistance = currentEdge.Distance + neighbor.Route.Value;
                if (newDistance < neighbor.Distance)
                {
                    neighbor.Distance = newDistance;
                    neighbor.PreviousRoute = currentEdge.Route;
                    queue.Enqueue(neighbor);
                }
            }
        }

        var edgeWithTheBestDestination = edges
            .Where(e => e.IsProcessed && e.Route.Destination == destination)
            .MinBy(e => e.Distance);

        if (edgeWithTheBestDestination is null)
            throw new InvalidRouteException([ "Rota inexistente." ]);

        var bufferPathToComposeTheFullRoute = new List<SingleRoute>();
        var currentPathNode = edgeWithTheBestDestination;
        do
        {
            bufferPathToComposeTheFullRoute.Add(currentPathNode.Route);
            currentPathNode = currentPathNode.PreviousRoute is not null ?
                edges.Single(e => e.Route.Id == currentPathNode.PreviousRoute.Id) :
                null;
        }
        while (currentPathNode is not null);
        bufferPathToComposeTheFullRoute.Reverse();

        return bufferPathToComposeTheFullRoute.Count == 1 ?
            bufferPathToComposeTheFullRoute.Single() : 
            new ConnectedRoute(bufferPathToComposeTheFullRoute);
    }
}
