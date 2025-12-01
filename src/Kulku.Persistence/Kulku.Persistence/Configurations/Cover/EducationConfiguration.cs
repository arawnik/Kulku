using Kulku.Domain.Cover;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Cover;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UuidGeneratedOnAdd();

        builder.Property(e => e.StartDate).IsRequired();
        builder.Property(e => e.EndDate).IsRequired();

        builder
            .HasMany(e => e.Translations)
            .WithOne(t => t.Education)
            .HasForeignKey(t => t.EducationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(e => e.Institution)
            .WithMany()
            .HasForeignKey(e => e.InstitutionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexing for performance
        builder.HasIndex(e => e.EndDate);
    }
}
