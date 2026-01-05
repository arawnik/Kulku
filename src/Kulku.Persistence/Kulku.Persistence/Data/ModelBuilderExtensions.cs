using System.Diagnostics.CodeAnalysis;
using Kulku.Domain;
using Kulku.Persistence.Converters;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kulku.Persistence.Data;

/// <summary>
/// Provides extension methods for configuring entity models.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class ModelBuilderExtensions
{
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

    /// <summary>
    /// Applies a value converter to all enum properties across the model.
    /// <list type="bullet">
    /// <item>
    /// If a property already has a converter configured explicitly, it is left untouched.
    /// </item>
    /// <item>
    /// <see cref="LanguageCode"/> is mapped using <see cref="LanguageCodeValueConverter"/>.
    /// </item>
    /// <item>
    /// All other enums are mapped using <see cref="EnumMemberValueConverter{TEnum}"/>, storing a
    /// readable string representation instead of an integer.
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="modelBuilder">The EF Core model builder instance.</param>
    internal static void ApplyEnumConverters(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                // Respect explicit configuration.
                if (property.GetValueConverter() is not null)
                    continue;

                var enumType = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;
                if (enumType is null || !enumType.IsEnum)
                    continue;

                // Handle special cases
                if (enumType == typeof(LanguageCode))
                {
                    property.SetValueConverter(new LanguageCodeValueConverter());

                    property.SetMaxLength(2);
                    property.SetIsUnicode(false);

                    continue;
                }

                // Apply the general converter if we got here.
                var converterType = typeof(EnumMemberValueConverter<>).MakeGenericType(enumType);

                if (Activator.CreateInstance(converterType) is not ValueConverter converter)
                {
                    throw new InvalidOperationException(
                        $"Failed to create enum converter for '{enumType.FullName}'."
                    );
                }

                property.SetValueConverter(converter);
            }
        }
    }
}
