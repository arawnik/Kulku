using Kulku.Domain;
using Kulku.Domain.Ideas;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaTagConfiguration : IEntityTypeConfiguration<IdeaTag>
{
    public void Configure(EntityTypeBuilder<IdeaTag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).UuidGeneratedOnAdd();

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.ColorHex).HasMaxLength(9);

        builder
            .HasMany(t => t.IdeaIdeaTags)
            .WithOne(it => it.IdeaTag)
            .HasForeignKey(it => it.IdeaTagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
