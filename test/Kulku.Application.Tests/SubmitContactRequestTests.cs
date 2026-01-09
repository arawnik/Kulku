using Kulku.Application.Abstractions.Security;
using Kulku.Application.Contacts;
using Kulku.Application.Contacts.Models;
using Kulku.Domain.Contacts;
using Kulku.Domain.Repositories;
using Kulku.Domain.Resources;
using Moq;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Application.Tests;

public class SubmitContactRequestTests
{
    private static (
        ICommandHandler<SubmitContactRequest.Command> handler,
        Mock<IRecaptchaValidator> recaptchaMock,
        Mock<IContactRequestRepository> repoMock,
        Mock<IUnitOfWork> uowMock
    ) CreateHandler(bool recaptchaValid)
    {
        var recaptchaMock = new Mock<IRecaptchaValidator>();
        recaptchaMock
            .Setup(r => r.ValidateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(recaptchaValid);

        var repoMock = new Mock<IContactRequestRepository>();

        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var handler = new SubmitContactRequest.Handler(
            recaptchaMock.Object,
            uowMock.Object,
            repoMock.Object
        );

        return (handler, recaptchaMock, repoMock, uowMock);
    }

    private static ContactRequestDto CreateSampleDto(string? captchaToken = "token") =>
        new("John", "john@example.com", "Hello", "Message", captchaToken!);

    [Fact]
    public async Task HandleReturnsSuccessWhenRecaptchaValidAndRepositoryCalled()
    {
        // Arrange
        var (handler, recaptchaMock, repoMock, uowMock) = CreateHandler(true);
        ContactRequest? captured = null;
        repoMock
            .Setup(r => r.Add(It.IsAny<ContactRequest>()))
            .Callback<ContactRequest>(c => captured = c);

        var dto = CreateSampleDto();

        // Act
        var result = await handler.Handle(
            new SubmitContactRequest.Command(dto),
            CancellationToken.None
        );

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(captured);
        Assert.Equal("John", captured!.Name);
        Assert.Equal("john@example.com", captured.Email);
        Assert.Equal("Hello", captured.Subject);
        Assert.Equal("Message", captured.Message);
        uowMock.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleReturnsBusinessRuleErrorWhenRecaptchaInvalid()
    {
        // Arrange
        var (handler, _, _, _) = CreateHandler(false);
        var dto = CreateSampleDto();

        // Act
        var result = await handler.Handle(
            new SubmitContactRequest.Command(dto),
            CancellationToken.None
        );

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(Strings.InvalidReCAPTCHA, result.Error!.Message);
    }

    [Fact]
    public void HandleThrowsArgumentNullExceptionWhenCaptchaTokenIsNull()
    {
        // Arrange
        var (handler, _, _, _) = CreateHandler(true);
        var dto = CreateSampleDto(null);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            handler
                .Handle(new SubmitContactRequest.Command(dto), CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        });
    }
}
