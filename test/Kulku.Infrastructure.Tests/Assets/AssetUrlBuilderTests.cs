using Kulku.Application.Abstractions.Assets;
using Kulku.Infrastructure.Assets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;

namespace Kulku.Infrastructure.Tests.Assets;

public sealed class AssetUrlBuilderTests
{
    [Fact]
    public void ProjectReturnsCorrectUrl()
    {
        var builder = CreateBuilder(baseUrl: "https://cdn.example.com/static");

        Assert.Equal(
            "https://cdn.example.com/static/projects/image.png",
            builder.Project("image.png")
        );
    }

    [Fact]
    public void IntroductionReturnsCorrectUrl()
    {
        var builder = CreateBuilder(baseUrl: "https://cdn.example.com/static");

        Assert.Equal(
            "https://cdn.example.com/static/introductions/avatar.webp",
            builder.Introduction("avatar.webp")
        );
    }

    [Fact]
    public void TrailingSlashIsTrimmed()
    {
        var builder = CreateBuilder(baseUrl: "https://cdn.example.com/static/");

        Assert.Equal(
            "https://cdn.example.com/static/introductions/avatar.jpg",
            builder.Introduction("avatar.jpg")
        );
    }

    [Fact]
    public void UriEncodesFileName()
    {
        var builder = CreateBuilder(baseUrl: "https://cdn.example.com/static");

        Assert.Equal(
            "https://cdn.example.com/static/projects/my%20image%20%281%29.png",
            builder.Project("my image (1).png")
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ThrowsOnNullOrEmptyFileName(string? fileName)
    {
        var builder = CreateBuilder(baseUrl: "https://cdn.example.com/static");

        Assert.ThrowsAny<ArgumentException>(() => builder.Project(fileName!));
    }

    [Fact]
    public void UsesBaseUrlWhenLocalPathIsNull()
    {
        var builder = CreateBuilder(baseUrl: "https://cdn.example.com/static");

        Assert.Equal(
            "https://cdn.example.com/static/projects/test.png",
            builder.Project("test.png")
        );
        Assert.Null(builder.LocalAssetPath);
    }

    [Fact]
    public void UsesBaseUrlWhenLocalPathDirectoryDoesNotExist()
    {
        var builder = CreateBuilder(
            localPath: "/nonexistent/path",
            baseUrl: "https://cdn.example.com/static"
        );

        Assert.Equal(
            "https://cdn.example.com/static/projects/test.png",
            builder.Project("test.png")
        );
        Assert.Null(builder.LocalAssetPath);
    }

    [Fact]
    public void UsesLocalPathWhenDirectoryExists()
    {
        using var tempDir = TempDirectory.Create();

        var builder = CreateBuilder(localPath: tempDir.FullPath);

        Assert.Equal("/static/projects/test.png", builder.Project("test.png"));
        Assert.Equal(tempDir.FullPath, builder.LocalAssetPath);
    }

    [Fact]
    public void ResolvesRelativeLocalPath()
    {
        using var contentRoot = TempDirectory.Create();
        var assetDirectory = contentRoot.CreateSubdirectory(Guid.NewGuid().ToString("N"));

        var builder = CreateBuilder(
            localPath: assetDirectory.Name,
            contentRootPath: contentRoot.FullPath
        );

        Assert.Equal("/static/introductions/avatar.jpg", builder.Introduction("avatar.jpg"));
        Assert.Equal(assetDirectory.FullName, builder.LocalAssetPath);
    }

    [Fact]
    public void ThrowsWhenNeitherSourceIsConfigured()
    {
        Assert.Throws<InvalidOperationException>(() => CreateBuilder());
    }

    [Fact]
    public void LocalPathTakesPrecedenceOverBaseUrl()
    {
        using var tempDir = TempDirectory.Create();

        var builder = CreateBuilder(
            localPath: tempDir.FullPath,
            baseUrl: "https://cdn.example.com/static"
        );

        Assert.Equal("/static/projects/test.png", builder.Project("test.png"));
        Assert.Equal(tempDir.FullPath, builder.LocalAssetPath);
    }

    private static AssetUrlBuilder CreateBuilder(
        string? localPath = null,
        string? baseUrl = null,
        string? contentRootPath = null
    )
    {
        var options = Options.Create(new AssetOptions { LocalPath = localPath, BaseUrl = baseUrl });

        var env = new Mock<IHostEnvironment>();
        env.Setup(e => e.ContentRootPath).Returns(contentRootPath ?? Path.GetTempPath());

        return new AssetUrlBuilder(options, env.Object);
    }

    /// <summary>
    /// Creates a temporary directory and deletes it on disposal.
    /// </summary>
    private sealed class TempDirectory : IDisposable
    {
        private readonly DirectoryInfo _directory;
        private bool _disposed;

        private TempDirectory(DirectoryInfo directory)
        {
            _directory = directory;
        }

        public string FullPath => _directory.FullName;

        public static TempDirectory Create(string prefix = "kulku-tests-") =>
            new(Directory.CreateTempSubdirectory(prefix));

        public DirectoryInfo CreateSubdirectory(string relativePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
            return _directory.CreateSubdirectory(relativePath);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (Directory.Exists(_directory.FullName))
            {
                Directory.Delete(_directory.FullName, recursive: true);
            }
        }
    }
}
