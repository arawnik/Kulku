using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Persistence.Data;

/// <summary>
/// Provides extension methods for configuring entity models.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ModelBuilderExtensions
{
    /// <summary>
    /// Configures entity relationships, including many-to-many relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance used for defining relationships.</param>
    internal static void ConfigureRelationships(this ModelBuilder modelBuilder) { }

    /// <summary>
    /// Configures Identity-related table mappings.
    /// </summary>
    internal static void ConfigureIdentityTables(this ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));

        builder.Entity<DataProtectionKey>().ToTable("DataProtectionKey");

        builder.Entity<ApplicationUser>().ToTable("User");
        builder.Entity<IdentityRole>().ToTable("Role");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
    }
}
