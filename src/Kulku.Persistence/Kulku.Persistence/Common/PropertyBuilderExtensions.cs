using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kulku.Persistence.Common;

public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Configures a Guid property to auto-generate using PostgreSQL's gen_random_uuid().
    /// </summary>
    public static PropertyBuilder<Guid> UuidGeneratedOnAdd(this PropertyBuilder<Guid> builder)
    {
        return builder.ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
    }
}
