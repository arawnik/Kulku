using Kulku.Domain;
using Kulku.Domain.Ideas;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaConfiguration : IEntityTypeConfiguration<Idea>
{
    public void Configure(EntityTypeBuilder<Idea> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).UuidGeneratedOnAdd();

        builder.Property(i => i.Title).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(i => i.Summary).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(i => i.Description).HasMaxLength(Defaults.TextAreaLength);
        builder.Property(i => i.StatusId).IsRequired();
        builder.Property(i => i.PriorityId).IsRequired();
        builder.Property(i => i.CreatedAt).IsRequired();
        builder.Property(i => i.UpdatedAt).IsRequired();

        builder
            .HasMany(i => i.Notes)
            .WithOne(n => n.Idea)
            .HasForeignKey(n => n.IdeaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(i => i.IdeaIdeaTags)
            .WithOne(it => it.Idea)
            .HasForeignKey(it => it.IdeaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
