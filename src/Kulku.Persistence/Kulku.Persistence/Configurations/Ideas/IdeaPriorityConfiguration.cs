using Kulku.Domain;
using Kulku.Domain.Ideas;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaPriorityConfiguration : IEntityTypeConfiguration<IdeaPriority>
{
    public void Configure(EntityTypeBuilder<IdeaPriority> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UuidGeneratedOnAdd();

        builder.Property(p => p.Order).IsRequired();
        builder.Property(p => p.Style).IsRequired().HasMaxLength(Defaults.StyleFieldLength);

        builder
            .HasMany(p => p.Translations)
            .WithOne(t => t.IdeaPriority)
            .HasForeignKey(t => t.IdeaPriorityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(p => p.Ideas)
            .WithOne(i => i.Priority)
            .HasForeignKey(i => i.PriorityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
