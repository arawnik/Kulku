using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Shared validation logic for idea create and update commands.
/// </summary>
internal static class IdeaCommandValidator
{
    public static Error[] Validate(string title, Guid domainId, Guid statusId, Guid priorityId)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(Error.Validation(nameof(title), Strings.Validation_TitleRequired));

        if (domainId == Guid.Empty)
            errors.Add(Error.Validation(nameof(domainId), Strings.Validation_DomainRequired));

        if (statusId == Guid.Empty)
            errors.Add(Error.Validation(nameof(statusId), Strings.Validation_StatusRequired));

        if (priorityId == Guid.Empty)
            errors.Add(Error.Validation(nameof(priorityId), Strings.Validation_PriorityRequired));

        return [.. errors];
    }
}
