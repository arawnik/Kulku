using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoulNETLib.Common.Extension;

namespace Kulku.Persistence.Converters;

/// <summary>
/// A custom EF Core <see cref="ValueConverter{TModel,TProvider}"/> that maps enum values to their
/// <see cref="EnumMemberAttribute.Value"/> when saving to the database, and parses them back
/// when reading from the database.
/// </summary>
/// <typeparam name="TEnum">The enum type being converted.</typeparam>
public class EnumMemberValueConverter<TEnum> : ValueConverter<TEnum, string>
    where TEnum : struct, Enum
{
    /// <summary>
    /// Configures conversion rules for mapping enum values to and from database-friendly strings.
    /// </summary>
    public EnumMemberValueConverter()
        : base(enumValue => enumValue.GetEnumMember(), stringValue => ParseEnumMember(stringValue))
    { }

    /// <summary>
    /// Parses the database string value into a corresponding enum value using <see cref="EnumMemberAttribute"/>.
    /// </summary>
    /// <param name="stringValue">The value stored in the database.</param>
    /// <returns>The corresponding enum value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the value does not match any EnumMemberAttribute.</exception>
    private static TEnum ParseEnumMember(string stringValue)
    {
        if (!stringValue.TryParseEnumMember<TEnum>(out var result))
            throw new InvalidOperationException(
                $"Cannot parse '{stringValue}' into {typeof(TEnum).Name} via EnumMemberAttribute."
            );
        return result.Value;
    }
}
