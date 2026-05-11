using Kulku.Domain;
using Kulku.Domain.Ideas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaPriorityTranslationConfiguration
    : IEntityTypeConfiguration<IdeaPriorityTranslation>
{
    public void Configure(EntityTypeBuilder<IdeaPriorityTranslation> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Description).HasMaxLength(Defaults.TextAreaLength);
        builder.Property(t => t.Language).IsRequired();

        builder.HasIndex(t => new { t.IdeaPriorityId, t.Language }).IsUnique();
    }
}
