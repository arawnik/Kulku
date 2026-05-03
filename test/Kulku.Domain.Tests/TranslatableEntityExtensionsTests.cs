using Kulku.Domain.Abstractions;

namespace Kulku.Domain.Tests;

public class TranslatableEntityExtensionsTests
{
    private sealed class FakeTranslation : ITranslationEntity
    {
        public LanguageCode Language { get; set; }
        public string Value { get; set; } = string.Empty;
    }

    private sealed record FakeDto(LanguageCode Language, string Value) : ITranslationDto;

    private sealed class FakeEntity : ITranslatableEntity<FakeTranslation>
    {
        public ICollection<FakeTranslation> Translations { get; init; } = [];
    }

    private static void Apply(FakeDto dto, FakeTranslation t) => t.Value = dto.Value;

    [Fact]
    public void MergeTranslationsCreatesNewWhenLanguageDoesNotExist()
    {
        var entity = new FakeEntity();
        var incoming = new List<FakeDto> { new(LanguageCode.English, "Hello") };

        entity.MergeTranslations(incoming, Apply);

        var translation = Assert.Single(entity.Translations);
        Assert.Equal(LanguageCode.English, translation.Language);
        Assert.Equal("Hello", translation.Value);
    }

    [Fact]
    public void MergeTranslationsUpdatesExistingWhenLanguageMatches()
    {
        var existing = new FakeTranslation { Language = LanguageCode.English, Value = "Old" };
        var entity = new FakeEntity { Translations = { existing } };
        var incoming = new List<FakeDto> { new(LanguageCode.English, "New") };

        entity.MergeTranslations(incoming, Apply);

        var translation = Assert.Single(entity.Translations);
        Assert.Same(existing, translation);
        Assert.Equal("New", translation.Value);
    }

    [Fact]
    public void MergeTranslationsRemovesTranslationWhenLanguageNotInIncoming()
    {
        var entity = new FakeEntity
        {
            Translations =
            {
                new FakeTranslation { Language = LanguageCode.English, Value = "EN" },
                new FakeTranslation { Language = LanguageCode.Finnish, Value = "FI" },
            },
        };
        var incoming = new List<FakeDto> { new(LanguageCode.English, "EN updated") };

        entity.MergeTranslations(incoming, Apply);

        var translation = Assert.Single(entity.Translations);
        Assert.Equal(LanguageCode.English, translation.Language);
        Assert.Equal("EN updated", translation.Value);
    }

    [Fact]
    public void MergeTranslationsClearsAllWhenIncomingIsEmpty()
    {
        var entity = new FakeEntity
        {
            Translations =
            {
                new FakeTranslation { Language = LanguageCode.English, Value = "EN" },
            },
        };

        entity.MergeTranslations(new List<FakeDto>(), Apply);

        Assert.Empty(entity.Translations);
    }

    [Fact]
    public void MergeTranslationsUpdatesExistingAndCreatesNewInMixedScenario()
    {
        var existingEn = new FakeTranslation { Language = LanguageCode.English, Value = "EN old" };
        var entity = new FakeEntity { Translations = { existingEn } };
        var incoming = new List<FakeDto>
        {
            new(LanguageCode.English, "EN new"),
            new(LanguageCode.Finnish, "FI new"),
        };

        entity.MergeTranslations(incoming, Apply);

        Assert.Equal(2, entity.Translations.Count);

        var en = entity.Translations.Single(t => t.Language == LanguageCode.English);
        Assert.Same(existingEn, en);
        Assert.Equal("EN new", en.Value);

        var fi = entity.Translations.Single(t => t.Language == LanguageCode.Finnish);
        Assert.NotSame(existingEn, fi);
        Assert.Equal("FI new", fi.Value);
    }
}
