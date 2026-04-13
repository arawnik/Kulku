using Kulku.Domain;
using Kulku.Domain.Network;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Network;

public class NetworkContactConfiguration : IEntityTypeConfiguration<NetworkContact>
{
    public void Configure(EntityTypeBuilder<NetworkContact> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).UuidGeneratedOnAdd();

        builder.Property(c => c.PersonName).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(c => c.Email).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(c => c.Phone).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(c => c.LinkedInUrl).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(c => c.Title).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(c => c.CreatedAt).IsRequired();

        builder
            .HasOne(c => c.Company)
            .WithMany()
            .HasForeignKey(c => c.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
