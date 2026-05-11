using Kulku.Domain;
using Kulku.Domain.Ideas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaDomainTranslationConfiguration : IEntityTypeConfiguration<IdeaDomainTranslation>
{
    public void Configure(EntityTypeBuilder<IdeaDomainTranslation> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Language).IsRequired();

        builder.HasIndex(t => new { t.IdeaDomainId, t.Language }).IsUnique();
    }
}
