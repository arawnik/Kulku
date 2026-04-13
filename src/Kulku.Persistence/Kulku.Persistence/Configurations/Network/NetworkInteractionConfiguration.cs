using Kulku.Domain;
using Kulku.Domain.Network;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Network;

public class NetworkInteractionConfiguration : IEntityTypeConfiguration<NetworkInteraction>
{
    public void Configure(EntityTypeBuilder<NetworkInteraction> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).UuidGeneratedOnAdd();

        builder.Property(i => i.Date).IsRequired();
        builder.HasIndex(i => i.Date);

        builder.Property(i => i.Summary).IsRequired().HasMaxLength(Defaults.TextAreaLength);
        builder.Property(i => i.NextAction).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(i => i.ReferredByName).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(i => i.ReferredByRelation).HasMaxLength(Defaults.TextFieldLength);
        builder.Property(i => i.CreatedAt).IsRequired();

        builder
            .HasOne(i => i.Company)
            .WithMany()
            .HasForeignKey(i => i.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(i => i.Contact)
            .WithMany()
            .HasForeignKey(i => i.ContactId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
