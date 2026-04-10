using Kulku.Domain.Network;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Network;

public class CompanyNetworkProfileCategoryConfiguration
    : IEntityTypeConfiguration<CompanyNetworkProfileCategory>
{
    public void Configure(EntityTypeBuilder<CompanyNetworkProfileCategory> builder)
    {
        builder.HasKey(pc => new { pc.CompanyNetworkProfileId, pc.NetworkCategoryId });
    }
}
