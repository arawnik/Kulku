using Kulku.Domain.Cover;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Cover;

public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> builder)
    {
        builder.HasKey(i => i.Id);

        builder
            .HasMany(i => i.Translations)
            .WithOne(t => t.Institution)
            .HasForeignKey(t => t.InstitutionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
