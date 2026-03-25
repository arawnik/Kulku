using Microsoft.AspNetCore.Components;

namespace Kulku.Web.Admin.Components;

public abstract class ApplicationComponentBase : ComponentBase, IDisposable, IAsyncDisposable
{
    private bool isAsyncDisposed;
    private bool isDisposed;

    private CancellationTokenSource? cancellationTokenSource;

    protected CancellationToken CancellationToken => (cancellationTokenSource ??= new()).Token;

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
