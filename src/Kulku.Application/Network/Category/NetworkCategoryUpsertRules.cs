using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Category;

/// <summary>
/// Shared validation rules for network category create and update commands.
/// </summary>
internal static class NetworkCategoryUpsertRules
{
    public static Error[] Validate(string name)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(Error.Validation(nameof(name), Strings.Validation_CategoryNameRequired));

        return [.. errors];
    }
}
