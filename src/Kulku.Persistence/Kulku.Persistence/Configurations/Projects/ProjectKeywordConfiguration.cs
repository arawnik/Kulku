using Kulku.Domain.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Configurations.Projects;

public class ProjectKeywordConfiguration : IEntityTypeConfiguration<ProjectKeyword>
{
    public void Configure(EntityTypeBuilder<ProjectKeyword> builder)
    {
        builder.HasKey(pk => new { pk.ProjectId, pk.KeywordId });

        builder
            .HasOne(pk => pk.Project)
            .WithMany(p => p.ProjectKeywords)
            .HasForeignKey(pk => pk.ProjectId);

        builder
            .HasOne(pk => pk.Keyword)
            .WithMany(k => k.ProjectKeywords)
            .HasForeignKey(pk => pk.KeywordId);
    }
}
