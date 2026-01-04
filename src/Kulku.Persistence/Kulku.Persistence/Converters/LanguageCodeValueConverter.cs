using Kulku.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kulku.Persistence.Converters;

/// <summary>
/// EF Core value converter for persisting <see cref="LanguageCode"/> values as
/// stable, database-friendly string codes.
/// </summary>
public sealed class LanguageCodeValueConverter : ValueConverter<LanguageCode, string>
{
    /// <summary>
    /// Database representation for English.
    /// </summary>
    private const string EnglishCode = "EN";

    /// <summary>
    /// Database representation for Finnish.
    /// </summary>
    private const string FinnishCode = "FI";

    /// <summary>
    /// Initializes a new <see cref="LanguageCodeValueConverter"/> with
    /// bidirectional mappings between <see cref="LanguageCode"/> and its
    /// persisted string representation.
    /// </summary>
    public LanguageCodeValueConverter()
        : base(model => ToDatabase(model), provider => FromDatabase(provider)) { }

    /// <summary>
    /// Converts a domain <see cref="LanguageCode"/> value into its database string representation.
    /// </summary>
    /// <param name="value">The domain language code.</param>
    /// <returns>The corresponding database value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when an unsupported <see cref="LanguageCode"/> is encountered.
    /// </exception>
    private static string ToDatabase(LanguageCode value) =>
        value switch
        {
            LanguageCode.English => EnglishCode,
            LanguageCode.Finnish => FinnishCode,
            _ => throw new ArgumentOutOfRangeException(
                nameof(value),
                value,
                "Unsupported LanguageCode."
            ),
        };

    /// <summary>
    /// Converts a database string value into its corresponding domain <see cref="LanguageCode"/>.
    /// </summary>
    /// <param name="value">The value read from the database.</param>
    /// <returns>The corresponding <see cref="LanguageCode"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the database value does not map to a supported language.
    /// </exception>
    private static LanguageCode FromDatabase(string value) =>
        value.Trim().ToUpperInvariant() switch
        {
            EnglishCode => LanguageCode.English,
            FinnishCode => LanguageCode.Finnish,
            _ => throw new InvalidOperationException($"Unsupported LanguageCode value '{value}'."),
        };
}
