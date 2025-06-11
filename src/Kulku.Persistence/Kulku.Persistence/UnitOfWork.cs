using Kulku.Persistence.Data;
using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Persistence;

/// <summary>
/// Implements the Unit of Work pattern to coordinate database operations.
/// This ensures that multiple repository operations are committed as a single transaction.
/// </summary>
public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Commits all pending changes in the database context.
    ///
    /// This method saves all tracked entity changes to the database in a single transaction.
    /// If a failure occurs, none of the changes will be persisted.
    ///
    /// Changes are tracked automatically, but you can manually detect changes
    /// using <c>_context.ChangeTracker.DetectChanges()</c> if needed.
    /// </summary>
    /// <param name="cancellationToken">
    /// A cancellation token to observe while waiting for the task to complete.
    /// Defaults to <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// The number of state entries written to the database.
    /// </returns>
    /// <exception cref="DbUpdateException">
    /// Thrown when an error occurs while saving changes to the database.
    /// </exception>
    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        //_context.ChangeTracker.DetectChanges();
        //Console.WriteLine(_context.ChangeTracker.DebugView.LongView);
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
