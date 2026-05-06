using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.IdeaBank;

/// <summary>
/// Shared validation rules for idea tag create and update commands.
/// </summary>
internal static class IdeaTagUpsertRules
{
    public static Error[] Validate(string name)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(Error.Validation(nameof(name), Strings.Validation_TagNameRequired));

        return [.. errors];
    }
}
