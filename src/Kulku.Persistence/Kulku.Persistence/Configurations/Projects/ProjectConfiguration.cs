using Kulku.Domain.Projects;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Projects;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(k => k.Id).UuidGeneratedOnAdd();

        builder.Property(p => p.Url).IsRequired();
        builder.Property(p => p.ImageUrl).IsRequired();
        builder.Property(p => p.Order).IsRequired();

        builder
            .HasMany(p => p.Translations)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(p => p.ProjectKeywords)
            .WithOne(pk => pk.Project)
            .HasForeignKey(pk => pk.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
