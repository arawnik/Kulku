using System.Globalization;
using Kulku.Application.Abstractions.Localization;
using Kulku.Domain;

namespace Kulku.Application.Tests;

public class LanguageCodeMapperTests
{
    [Theory]
    [InlineData("en-US", LanguageCode.English)]
    [InlineData("en", LanguageCode.English)]
    [InlineData("fi-FI", LanguageCode.Finnish)]
    [InlineData("fi", LanguageCode.Finnish)]
    [InlineData("sv-SE", LanguageCode.English)] // unsupported -> default
    public void FromCultureMapsKnownCulturesOrDefaults(string cultureName, LanguageCode expected)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);

        // Act
        var result = LanguageCodeMapper.FromCulture(culture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FromCultureNullReturnsDefault()
    {
        // Arrange & Act
        var result = LanguageCodeMapper.FromCulture(null!);

        // Assert
        Assert.Equal(Defaults.Language, result);
    }

    [Theory]
    [InlineData("EN-US", LanguageCode.English)]
    [InlineData("Fi-fi", LanguageCode.Finnish)]
    public void FromCultureIsCaseInsensitive(string cultureName, LanguageCode expected)
    {
        // Arrange
        var culture = new CultureInfo(cultureName);

        // Act
        var result = LanguageCodeMapper.FromCulture(culture);

        // Assert
        Assert.Equal(expected, result);
    }
}
