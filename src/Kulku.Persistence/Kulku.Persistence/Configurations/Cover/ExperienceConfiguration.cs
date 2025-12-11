using Kulku.Domain.Cover;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Cover;

public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UuidGeneratedOnAdd();

        builder
            .HasMany(e => e.Translations)
            .WithOne(t => t.Experience)
            .HasForeignKey(t => t.ExperienceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(e => e.Company)
            .WithMany()
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Keywords).WithMany();

        // Indexing for performance
        builder.HasIndex(e => e.EndDate);
    }
}
