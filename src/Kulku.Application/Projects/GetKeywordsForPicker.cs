using Kulku.Application.Projects.Models;
using Kulku.Application.Projects.Ports;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Projects;

/// <summary>
/// Retrieves all keywords as lightweight picker models for admin forms.
/// </summary>
public static class GetKeywordsForPicker
{
    public sealed record Query() : IQuery<IReadOnlyList<KeywordPickerModel>>;

    internal sealed class Handler(IKeywordQueries queries)
        : IQueryHandler<Query, IReadOnlyList<KeywordPickerModel>>
    {
        private readonly IKeywordQueries _queries = queries;

        public async Task<Result<IReadOnlyList<KeywordPickerModel>>> Handle(
            Query query,
            CancellationToken cancellationToken
        )
        {
            return Result.Success(await _queries.ListAllForPickerAsync(cancellationToken));
        }
    }
}
