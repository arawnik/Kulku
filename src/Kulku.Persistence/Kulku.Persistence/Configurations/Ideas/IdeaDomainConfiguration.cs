using Kulku.Domain;
using Kulku.Domain.Ideas;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaDomainConfiguration : IEntityTypeConfiguration<IdeaDomain>
{
    public void Configure(EntityTypeBuilder<IdeaDomain> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).UuidGeneratedOnAdd();

        builder.Property(d => d.Icon).IsRequired().HasMaxLength(Defaults.StyleFieldLength);
        builder.Property(d => d.Order).IsRequired();

        builder
            .HasMany(d => d.Translations)
            .WithOne(t => t.IdeaDomain)
            .HasForeignKey(t => t.IdeaDomainId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(d => d.Ideas)
            .WithOne(i => i.Domain)
            .HasForeignKey(i => i.DomainId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
