using Kulku.Domain.Ideas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Ideas;

public class IdeaKeywordConfiguration : IEntityTypeConfiguration<IdeaKeyword>
{
    public void Configure(EntityTypeBuilder<IdeaKeyword> builder)
    {
        builder.HasKey(ik => new { ik.IdeaId, ik.KeywordId });

        builder.HasOne(ik => ik.Idea).WithMany(i => i.IdeaKeywords).HasForeignKey(ik => ik.IdeaId);

        builder
            .HasOne(ik => ik.Keyword)
            .WithMany(k => k.IdeaKeywords)
            .HasForeignKey(ik => ik.KeywordId);
    }
}
