using Kulku.Application.Resources;
using SoulNETLib.Clean.Domain;

namespace Kulku.Application.Network.Category;

/// <summary>
/// Shared validation logic for network category commands.
/// </summary>
internal static class NetworkCategoryCommandValidator
{
    public static Error[] Validate(string name)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(Error.Validation(nameof(name), Strings.Validation_CategoryNameRequired));

        return [.. errors];
    }
}
