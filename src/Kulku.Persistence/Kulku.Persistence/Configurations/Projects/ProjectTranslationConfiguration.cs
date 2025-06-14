using Kulku.Domain.Constants;
using Kulku.Domain.Projects;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Projects;

public class ProjectTranslationConfiguration : IEntityTypeConfiguration<ProjectTranslation>
{
    public void Configure(EntityTypeBuilder<ProjectTranslation> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(k => k.Id).UuidGeneratedOnAdd();

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Info).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Description).HasMaxLength(Defaults.TextAreaLength);

        builder.HasIndex(t => new { t.ProjectId, t.Language }).IsUnique();
    }
}
