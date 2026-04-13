using Kulku.Domain;
using Kulku.Domain.Cover;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Cover;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).UuidGeneratedOnAdd();

        builder.Property(c => c.Website).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(c => c.Region).HasMaxLength(Defaults.TextFieldLength);

        builder
            .HasMany(c => c.Translations)
            .WithOne(t => t.Company)
            .HasForeignKey(t => t.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
