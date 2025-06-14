using Kulku.Domain.Projects;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Projects;

public class ProficiencyConfiguration : IEntityTypeConfiguration<Proficiency>
{
    public void Configure(EntityTypeBuilder<Proficiency> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(k => k.Id).UuidGeneratedOnAdd();

        builder.Property(p => p.Scale).IsRequired();
        builder.Property(p => p.Order).IsRequired();

        builder
            .HasMany(p => p.Translations)
            .WithOne(t => t.Proficiency)
            .HasForeignKey(t => t.ProficiencyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(p => p.Keywords)
            .WithOne(k => k.Proficiency)
            .HasForeignKey(k => k.ProficiencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
