using Kulku.Domain;
using Kulku.Domain.Ideas;
using Kulku.Persistence.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaNoteConfiguration : IEntityTypeConfiguration<IdeaNote>
{
    public void Configure(EntityTypeBuilder<IdeaNote> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).UuidGeneratedOnAdd();

        builder.Property(n => n.Content).IsRequired().HasMaxLength(Defaults.TextAreaLength);
        builder.Property(n => n.CreatedAt).IsRequired();
    }
}
