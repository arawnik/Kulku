using Kulku.Domain.Cover;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Cover;

public class IntroductionConfiguration : IEntityTypeConfiguration<Introduction>
{
    public void Configure(EntityTypeBuilder<Introduction> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).UuidGeneratedOnAdd();

        builder.Property(i => i.AvatarUrl).IsRequired();
        builder.Property(i => i.SmallAvatarUrl).IsRequired();
        builder.Property(i => i.PubDate).IsRequired();

        builder
            .HasMany(i => i.Translations)
            .WithOne(t => t.Introduction)
            .HasForeignKey(t => t.IntroductionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexing for performance
        builder.HasIndex(e => e.PubDate);
    }
}
