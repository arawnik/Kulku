using Kulku.Domain;
using Kulku.Domain.Ideas;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaStatusConfiguration : IEntityTypeConfiguration<IdeaStatus>
{
    public void Configure(EntityTypeBuilder<IdeaStatus> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).UuidGeneratedOnAdd();

        builder.Property(s => s.Order).IsRequired();
        builder.Property(s => s.Style).IsRequired().HasMaxLength(Defaults.StyleFieldLength);

        builder
            .HasMany(s => s.Translations)
            .WithOne(t => t.IdeaStatus)
            .HasForeignKey(t => t.IdeaStatusId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(s => s.Ideas)
            .WithOne(i => i.Status)
            .HasForeignKey(i => i.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
