namespace BestRoute.Models;

public class ConnectedRoute : IRoute
{
    private ICollection<SingleRoute> Routes { get; set; }
    
    public string Origin => Routes.First().Origin;
    public string Destination => Routes.Last().Destination;

    public ConnectedRoute(IEnumerable<SingleRoute> singleRoutes)
    {
        Routes = singleRoutes.ToList();
        if (Routes.Count < 2)
            throw new InvalidRouteException([ "Uma conexão deve possuir no mínimo 2 rotas." ]);
    }

    public decimal GetValue() => Routes.Sum(r => r.Value);

    public IEnumerable<string> ListTerminals()
        => Routes.Select(r => r.Origin).Append(Destination);
}
