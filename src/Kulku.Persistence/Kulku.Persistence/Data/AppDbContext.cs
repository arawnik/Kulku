using System.Diagnostics.CodeAnalysis;
using Kulku.Domain.Contacts;
using Kulku.Domain.Cover;
using Kulku.Domain.Projects;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Persistence.Data;

/// <summary>
/// Represents the database context for the Holdion application.
/// Extends <see cref="HoldionDbContext"/> to support entity specific actions and multi-tenant (household-based) data scoping.
/// </summary>
[ExcludeFromCodeCoverage]
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Projects
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectTranslation> ProjectTranslations { get; set; } = null!;
    public DbSet<Keyword> Keywords { get; set; } = null!;
    public DbSet<KeywordTranslation> KeywordTranslations { get; set; } = null!;

    // Not required unless directly queried/searched
    // public DbSet<ProjectKeyword> ProjectKeywords { get; set; } = null!;
    public DbSet<Proficiency> Proficiencies { get; set; } = null!;
    public DbSet<ProficiencyTranslation> ProficiencyTranslations { get; set; } = null!;

    // Cover
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<CompanyTranslation> CompanyTranslations { get; set; } = null!;

    public DbSet<Education> Educations { get; set; } = null!;
    public DbSet<EducationTranslation> EducationTranslations { get; set; } = null!;

    public DbSet<Experience> Experiences { get; set; } = null!;
    public DbSet<ExperienceTranslation> ExperienceTranslations { get; set; } = null!;

    public DbSet<Institution> Institutions { get; set; } = null!;
    public DbSet<InstitutionTranslation> InstitutionTranslations { get; set; } = null!;

    public DbSet<Introduction> Introductions { get; set; } = null!;
    public DbSet<IntroductionTranslation> IntroductionTranslations { get; set; } = null!;

    public DbSet<ContactRequest> ContactRequests { get; set; } = null!;

    /// <summary>
    /// Configures the entity model for the database context.
    /// This method applies all entity relationships, constraints, and mappings using Fluent API,
    /// including domain-specific configuration and identity/table mapping extensions.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to construct the entity model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));

        //Call the base first.
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ConvertEnumsToString();

        // Apply Fluent API configurations
        modelBuilder.ConfigureRelationships();
    }
}
