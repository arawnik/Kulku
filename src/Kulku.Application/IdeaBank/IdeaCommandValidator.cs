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
            errors.Add(Error.Validation(nameof(title), "Title is required."));

        if (domainId == Guid.Empty)
            errors.Add(Error.Validation(nameof(domainId), "Domain is required."));

        if (statusId == Guid.Empty)
            errors.Add(Error.Validation(nameof(statusId), "Status is required."));

        if (priorityId == Guid.Empty)
            errors.Add(Error.Validation(nameof(priorityId), "Priority is required."));

        return [.. errors];
    }
}
