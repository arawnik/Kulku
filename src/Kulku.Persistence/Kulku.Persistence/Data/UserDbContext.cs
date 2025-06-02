using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Persistence.Data;

/// <summary>
/// Identity related user db context.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class UserDbContext(DbContextOptions<UserDbContext> options)
    : IdentityDbContext<ApplicationUser>(options),
        IDataProtectionKeyContext
{
    /// <summary>
    /// Stores keys for data protection (used for encrypting sensitive data).
    /// </summary>
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //Call the base first.
        base.OnModelCreating(builder);

        // Apply Identity table customizations (moved to extension method)
        builder.ConfigureIdentityTables();
    }
}
