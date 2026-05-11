using Kulku.Domain;
using Kulku.Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Projects;

public class ProficiencyTranslationConfiguration : IEntityTypeConfiguration<ProficiencyTranslation>
{
    public void Configure(EntityTypeBuilder<ProficiencyTranslation> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Description).HasMaxLength(Defaults.TextAreaLength);
        builder.Property(t => t.Language).IsRequired();

        builder.HasIndex(t => new { t.ProficiencyId, t.Language }).IsUnique();
    }
}
