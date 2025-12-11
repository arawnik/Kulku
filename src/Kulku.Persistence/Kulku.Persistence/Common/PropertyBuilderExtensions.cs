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
        //TODO: This is an issue if trying another db provider than PgSql
        return builder.HasDefaultValueSql("gen_random_uuid()");
    }
}
