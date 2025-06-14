using Kulku.Domain.Projects;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Projects;

public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
{
    public void Configure(EntityTypeBuilder<Keyword> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).UuidGeneratedOnAdd();

        builder.Property(k => k.Type).IsRequired();
        builder.Property(k => k.Order).IsRequired();
        builder.Property(k => k.Display).IsRequired();

        builder
            .HasMany(k => k.Translations)
            .WithOne(t => t.Keyword)
            .HasForeignKey(t => t.KeywordId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(k => k.ProjectKeywords)
            .WithOne(pk => pk.Keyword)
            .HasForeignKey(pk => pk.KeywordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
