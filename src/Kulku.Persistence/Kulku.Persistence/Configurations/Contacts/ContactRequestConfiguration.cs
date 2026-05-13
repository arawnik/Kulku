using Kulku.Domain;
using Kulku.Domain.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Contacts;

public class ContactRequestConfiguration : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Email).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Subject).IsRequired().HasMaxLength(Defaults.TextFieldLength);
        builder.Property(t => t.Message).HasMaxLength(Defaults.TextAreaLength);
    }
}
