using Kulku.Domain.Cover;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Cover;

public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).UuidGeneratedOnAdd();

        builder
            .HasMany(i => i.Translations)
            .WithOne(t => t.Institution)
            .HasForeignKey(t => t.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
