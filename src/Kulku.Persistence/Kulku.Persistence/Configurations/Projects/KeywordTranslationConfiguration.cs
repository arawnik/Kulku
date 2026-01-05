using Kulku.Domain;
using Kulku.Domain.Projects;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Projects;

public class KeywordTranslationConfiguration : IEntityTypeConfiguration<KeywordTranslation>
{
    public void Configure(EntityTypeBuilder<KeywordTranslation> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(k => k.Id).UuidGeneratedOnAdd();

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Language).IsRequired();

        builder.HasIndex(t => new { t.KeywordId, t.Language }).IsUnique();
    }
}
