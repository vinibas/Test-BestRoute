namespace BestRoute.Models;

public class InvalidRouteException : Exception
{
    public IEnumerable<string> Errors { get; }

    public InvalidRouteException(IEnumerable<string> errors) : base("Rota inválida.")
        => Errors = errors;
}
