using BestRoute.Models;
using Microsoft.EntityFrameworkCore;

namespace BestRoute.Data.Repositories;

public class RouteRepository : IRouteRepository
{
    private Context Context { get; }

    public RouteRepository(Context context)
    {
        Context = context;
    }

    public async Task AddAsync(SingleRoute route)
        => await Context.Routes.AddAsync(route);

    public async Task<SingleRoute?> GetByIdAsync(Guid id)
        => await Context.Routes.FirstOrDefaultAsync(r => r.Id == id);

    public Task<IEnumerable<SingleRoute>> ListAsync()
        => ListAsync(null, null);

    public async Task<IEnumerable<SingleRoute>> ListAsync(string? origin, string? destination)
    {
        var query = Context.Routes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(origin))
            query = query.Where(r => r.Origin == origin);
        if (!string.IsNullOrWhiteSpace(destination))
            query = query.Where(r => r.Destination == destination);

        return await query.ToListAsync();
    }

    public void Delete(SingleRoute route)
        => Context.Routes.Remove(route);

    public Task SaveChangesAsync()
        => Context.SaveChangesAsync();
}
