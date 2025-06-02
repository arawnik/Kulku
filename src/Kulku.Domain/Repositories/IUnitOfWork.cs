namespace Kulku.Domain.Repositories;

/// <summary>
/// Defines the Unit of Work pattern for coordinating database transactions.
///
/// This interface ensures that multiple repository operations are executed within a single
/// transaction, preventing partial updates in case of failure.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits all pending changes in the database context.
    ///
    /// This method saves all tracked entity changes to the database in a single transaction.
    /// If a failure occurs, none of the changes will be persisted.
    /// </summary>
    /// <param name="cancellationToken">
    /// A cancellation token to observe while waiting for the task to complete.
    /// Defaults to <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// The number of state entries written to the database.
    /// </returns>
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
    /*Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> GetCurrentTransaction;*/
}
