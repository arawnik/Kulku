using Kulku.Domain.Constants;
using Kulku.Domain.Cover;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Cover;

public class EducationTranslationConfiguration : IEntityTypeConfiguration<EducationTranslation>
{
    public void Configure(EntityTypeBuilder<EducationTranslation> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).UuidGeneratedOnAdd();

        builder.Property(t => t.Title).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Description).HasMaxLength(Defaults.TextAreaLength);
        builder.Property(t => t.Language).IsRequired();

        // Indexing for performance
        builder.HasIndex(t => new { t.Language, t.EducationId }).IsUnique();
    }
}
