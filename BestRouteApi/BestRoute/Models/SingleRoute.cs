namespace BestRoute.Models;

public class SingleRoute : IRoute
{
    public Guid Id { get; }
    public string Origin { get; private set; }
    public string Destination { get; private set; }
    public decimal Value { get; private set; }

    #pragma warning disable CS8618 // For EF
    private SingleRoute() { }
    #pragma warning restore CS8618

    public SingleRoute(string origin, string destination, decimal value)
    {
        ValidateInvariants(origin, destination, value);

        Id = Guid.NewGuid();
        Origin = origin.ToUpper();
        Destination = destination.ToUpper();
        Value = value;
    }

    public decimal GetValue() => Value;
    
    public void Update(string origin, string destination, decimal value)
    {
        ValidateInvariants(origin, destination, value);

        Origin = origin.ToUpper();
        Destination = destination.ToUpper();
        Value = value;
    }

    private void ValidateInvariants(string origin, string destination, decimal value)
    {
        var errors = new List<string>();
        
        if (origin?.Length != 3)
            errors.Add("Origem deve ter 3 caracteres.");
        if (destination?.Length != 3)
            errors.Add("Destino deve ter 3 caracteres.");
        if (value <= 0)
            errors.Add("Valor deve ser maior que zero.");

        if (errors.Any())
            throw new InvalidRouteException(errors);
    }

    public IEnumerable<string> ListTerminals()
        => [ Origin, Destination ];
}
