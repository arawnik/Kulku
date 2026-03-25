using Kulku.Domain;
using Kulku.Domain.Ideas;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaStatusTranslationConfiguration : IEntityTypeConfiguration<IdeaStatusTranslation>
{
    public void Configure(EntityTypeBuilder<IdeaStatusTranslation> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).UuidGeneratedOnAdd();

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Description).HasMaxLength(Defaults.TextAreaLength);
        builder.Property(t => t.Language).IsRequired();

        builder.HasIndex(t => new { t.IdeaStatusId, t.Language }).IsUnique();
    }
}
