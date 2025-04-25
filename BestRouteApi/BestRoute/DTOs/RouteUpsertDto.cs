namespace BestRoute.DTOs;

public record RouteUpsertDto(string Origin, string Destination, decimal Value);

public record BestRoutesDto(IEnumerable<string> Routes, decimal Value);
