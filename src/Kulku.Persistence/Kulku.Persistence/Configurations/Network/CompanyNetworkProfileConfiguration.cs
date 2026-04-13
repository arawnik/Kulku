using Kulku.Domain;
using Kulku.Domain.Network;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Network;

public class CompanyNetworkProfileConfiguration : IEntityTypeConfiguration<CompanyNetworkProfile>
{
    public void Configure(EntityTypeBuilder<CompanyNetworkProfile> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UuidGeneratedOnAdd();

        builder.HasIndex(p => p.CompanyId).IsUnique();

        builder.Property(p => p.Notes).HasMaxLength(Defaults.TextAreaLength);

        builder
            .HasOne(p => p.Company)
            .WithMany()
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(p => p.CompanyNetworkProfileCategories)
            .WithOne(pc => pc.CompanyNetworkProfile)
            .HasForeignKey(pc => pc.CompanyNetworkProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
