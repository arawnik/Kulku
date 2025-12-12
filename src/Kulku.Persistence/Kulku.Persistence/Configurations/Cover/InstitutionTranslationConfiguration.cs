using Kulku.Domain.Constants;
using Kulku.Domain.Cover;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Cover;

public class InstitutionTranslationConfiguration : IEntityTypeConfiguration<InstitutionTranslation>
{
    public void Configure(EntityTypeBuilder<InstitutionTranslation> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).UuidGeneratedOnAdd();

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Department).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Description).HasMaxLength(Defaults.TextAreaLength);
        builder.Property(t => t.Language).IsRequired();

        // Indexing for performance
        builder.HasIndex(t => new { t.Language, t.InstitutionId }).IsUnique();
    }
}
