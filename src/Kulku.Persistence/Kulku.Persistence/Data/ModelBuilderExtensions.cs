using System.Diagnostics.CodeAnalysis;
using Kulku.Persistence.Configurations.Cover;
using Kulku.Persistence.Configurations.Projects;
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
    /// Configures entity relationships, including many-to-many relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance used for defining relationships.</param>
    internal static void ConfigureRelationships(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));

        // Project module
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectTranslationConfiguration());
        modelBuilder.ApplyConfiguration(new KeywordConfiguration());
        modelBuilder.ApplyConfiguration(new KeywordTranslationConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectKeywordConfiguration());
        modelBuilder.ApplyConfiguration(new ProficiencyConfiguration());
        modelBuilder.ApplyConfiguration(new ProficiencyTranslationConfiguration());

        // Cover module
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyTranslationConfiguration());
        modelBuilder.ApplyConfiguration(new EducationConfiguration());
        modelBuilder.ApplyConfiguration(new EducationTranslationConfiguration());
        modelBuilder.ApplyConfiguration(new ExperienceConfiguration());
        modelBuilder.ApplyConfiguration(new ExperienceTranslationConfiguration());
        modelBuilder.ApplyConfiguration(new InstitutionConfiguration());
        modelBuilder.ApplyConfiguration(new InstitutionTranslationConfiguration());
        modelBuilder.ApplyConfiguration(new IntroductionConfiguration());
        modelBuilder.ApplyConfiguration(new IntroductionTranslationConfiguration());
    }

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
    /// Applies a value converter to all enum properties across the model,
    /// ensuring enums are stored as strings in the database rather than integers.
    /// This makes the database more readable and avoids issues with enum value changes.
    /// </summary>
    /// <param name="modelBuilder">The EF Core model builder instance.</param>
    internal static void ConvertEnumsToString(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var enumType = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;

                if (enumType.IsEnum)
                {
                    var converterType = typeof(EnumMemberValueConverter<>).MakeGenericType(
                        enumType
                    );
                    var converter = (ValueConverter)Activator.CreateInstance(converterType)!;

                    property.SetValueConverter(converter);
                }
            }
        }
    }
}
