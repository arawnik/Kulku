using Kulku.Domain;
using Kulku.Domain.Network;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Network;

public class NetworkCategoryConfiguration : IEntityTypeConfiguration<NetworkCategory>
{
    public void Configure(EntityTypeBuilder<NetworkCategory> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).UuidGeneratedOnAdd();

        builder.Property(c => c.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(c => c.ColorToken).HasMaxLength(Defaults.StyleFieldLength);

        builder
            .HasMany(c => c.CompanyNetworkProfileCategories)
            .WithOne(pc => pc.NetworkCategory)
            .HasForeignKey(pc => pc.NetworkCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
