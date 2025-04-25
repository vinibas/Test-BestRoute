namespace BestRoute.Models;

public interface IRoute
{
    string Origin { get; }
    string Destination { get; }
    decimal GetValue();
    IEnumerable<string> ListTerminals();
}
