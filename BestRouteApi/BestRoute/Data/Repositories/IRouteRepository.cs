
namespace BestRoute.Data.Repositories;

using BestRoute.Models;

public interface IRouteRepository
{
    Task AddAsync(SingleRoute route);
    Task<IEnumerable<SingleRoute>> ListAsync();
    Task<IEnumerable<SingleRoute>> ListAsync(string? origin, string? destination);
    Task<SingleRoute?> GetByIdAsync(Guid id);
    void Delete(SingleRoute route);
    Task SaveChangesAsync();
}
