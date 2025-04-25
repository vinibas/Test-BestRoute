using System.Reflection;
using BestRoute.Models;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace BestRoute.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<SingleRoute> Routes { get; set; }
}