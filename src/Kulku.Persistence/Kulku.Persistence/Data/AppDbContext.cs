using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Persistence.Data;

/// <summary>
/// Represents the database context for the Holdion application.
/// Extends <see cref="HoldionDbContext"/> to support entity specific actions and multi-tenant (household-based) data scoping.
/// </summary>
[ExcludeFromCodeCoverage]
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    //General
    //public DbSet<Household> Households { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));

        //Call the base first.
        base.OnModelCreating(modelBuilder);

        // Apply Fluent API configurations
        modelBuilder.ConfigureRelationships();
    }
}
