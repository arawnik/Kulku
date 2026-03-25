using Kulku.Domain.Ideas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaIdeaTagConfiguration : IEntityTypeConfiguration<IdeaIdeaTag>
{
    public void Configure(EntityTypeBuilder<IdeaIdeaTag> builder)
    {
        builder.HasKey(it => new { it.IdeaId, it.IdeaTagId });
    }
}
