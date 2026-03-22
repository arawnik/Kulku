using SoulNETLib.Clean.Domain;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Shared validation logic for idea tag commands.
/// </summary>
internal static class IdeaTagCommandValidator
{
    public static Error[] Validate(string name)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(Error.Validation(nameof(name), "Tag name is required."));

        return [.. errors];
    }
}
