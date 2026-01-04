using Microsoft.EntityFrameworkCore;

namespace Kulku.Persistence.Data;

/// <summary>
/// Provides methods to initialize and seed the application’s database
/// with required default data on startup.
/// </summary>
/// <remarks>
/// This initializer will populate the database with essential seed data such as
/// lookup tables, default configuration values, and any required admin or system users.
/// </remarks>
public static class AppDbInitializer
{
    /// <summary>
    /// Ensures the database schema is up-to-date and seeds initial data.
    /// </summary>
    /// <param name="context">
    /// The <see cref="AppDbContext"/> used to insert seed data.
    /// </param>
    /// <exception cref="DbUpdateException">
    /// Thrown if an error occurs while applying migrations or saving seeded data.
    /// </exception>
    public static void Initialize(AppDbContext context)
    {
        context.Database.MigrateAsync();
    }
}
