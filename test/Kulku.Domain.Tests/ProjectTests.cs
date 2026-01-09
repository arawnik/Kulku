using Kulku.Domain.Projects;

namespace Kulku.Domain.Tests;

public class ProjectTests
{
    [Fact]
    public void ProjectDefaultsAreInitialized()
    {
        // Arrange & Act
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Url = new Uri("https://example.com"),
            ImageUrl = new Uri("https://example.com/image.png"),
        };

        // Assert
        Assert.NotNull(project.Translations);
        Assert.Empty(project.Translations);

        Assert.NotNull(project.ProjectKeywords);
        Assert.Empty(project.ProjectKeywords);

        Assert.Equal(1, project.Order);
    }

    [Fact]
    public void ProjectImplementsITranslatableEntity()
    {
        // Arrange & Act
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Url = new Uri("https://example.com"),
            ImageUrl = new Uri("https://example.com/image.png"),
        };

        // Assert
        Assert.True(project is Kulku.Domain.Abstractions.ITranslatableEntity<ProjectTranslation>);
    }
}
