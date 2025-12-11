using Kulku.Domain.Constants;
using Kulku.Domain.Contacts;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Contacts;

public class ContactRequestConfiguration : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).UuidGeneratedOnAdd();

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Email).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Subject).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Message).HasMaxLength(Defaults.TextAreaLength);
    }
}
