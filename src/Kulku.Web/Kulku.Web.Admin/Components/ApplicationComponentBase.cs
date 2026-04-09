using Kulku.Application.Abstractions.Localization;
using Kulku.Domain;
using Microsoft.AspNetCore.Components;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components;

public abstract class ApplicationComponentBase : ComponentBase, IDisposable, IAsyncDisposable
{
    private bool isAsyncDisposed;
    private bool isDisposed;

    private CancellationTokenSource? cancellationTokenSource;

    protected CancellationToken CancellationToken => (cancellationTokenSource ??= new()).Token;

    /// <summary>
    /// Creates a list of blank translation items for all supported languages using the provided factory.
    /// </summary>
    protected static IReadOnlyList<T> BuildBlankTranslations<T>(Func<LanguageCode, T> factory) =>
        [.. LanguageCodeMapper.SupportedLanguageCodes.Select(factory)];

    /// <summary>
    /// Inspects a <see cref="Result"/> for validation or general failures.
    /// Returns <c>true</c> if the result is a failure (handled), <c>false</c> if successful.
    /// </summary>
    protected static bool TryHandleError(
        Result result,
        Action<IEnumerable<Error>>? setServerErrors,
        ref string? errorMessage,
        string fallbackMessage
    )
    {
        if (result.IsSuccess)
            return false;

        if (result is IValidationResult validation)
        {
            setServerErrors?.Invoke(validation.Errors);
            return true;
        }

        errorMessage = result.Error?.Message ?? fallbackMessage;
        return true;
    }

    /// <summary>
    /// Inspects a <see cref="Result{T}"/> for validation or general failures.
    /// Returns <c>true</c> if the result is a failure (handled), <c>false</c> if successful.
    /// </summary>
    protected static bool TryHandleError<T>(
        Result<T> result,
        Action<IEnumerable<Error>>? setServerErrors,
        ref string? errorMessage,
        string fallbackMessage
    )
    {
        if (result.IsSuccess)
            return false;

        if (result is IValidationResult validation)
        {
            setServerErrors?.Invoke(validation.Errors);
            return true;
        }

        errorMessage = result.Error?.Message ?? fallbackMessage;
        return true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore(true).ConfigureAwait(false);

        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
                }
                OnDispose();
            }

            isDisposed = true;
        }
    }

    protected async ValueTask DisposeAsyncCore(bool disposing)
    {
        if (!isAsyncDisposed)
        {
            if (disposing)
            {
                if (cancellationTokenSource != null)
                {
                    await cancellationTokenSource.CancelAsync();
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
                }
                OnDispose();
            }

            isAsyncDisposed = true;
        }
    }

    protected virtual void OnDispose() { }
}
