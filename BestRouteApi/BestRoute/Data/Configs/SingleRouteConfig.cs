using BestRoute.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BestRoute.Data.Configs;

public class SingleSingleRouteConfig : IEntityTypeConfiguration<SingleRoute>
{
    public void Configure(EntityTypeBuilder<SingleRoute> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Origin).HasMaxLength(3).IsFixedLength();
    }
}
