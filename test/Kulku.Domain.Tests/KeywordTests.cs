using Kulku.Domain.Projects;

namespace Kulku.Domain.Tests;

public class KeywordTests
{
    [Fact]
    public void KeywordDefaultsAreInitialized()
    {
        // Arrange & Act
        var keyword = new Keyword();

        // Assert
        Assert.NotNull(keyword.Translations);
        Assert.Empty(keyword.Translations);

        Assert.NotNull(keyword.ProjectKeywords);
        Assert.Empty(keyword.ProjectKeywords);

        Assert.Equal(KeywordType.Skill, keyword.Type);
        Assert.Equal(1, keyword.Order);
        Assert.True(keyword.Display);
    }

    [Fact]
    public void KeywordTranslationDefaultLanguageIsDefaultsLanguage()
    {
        // arrange & Act
        var kt = new KeywordTranslation();

        // Assert
        Assert.Equal(Defaults.Language, kt.Language);
    }
}
