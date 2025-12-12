using SoulNETLib.Clean.Domain.Repositories;

namespace Kulku.Domain.Abstractions;

/// <summary>
/// Generic interface for the command side of CRUD operations on an entity.
/// It defines **base cases** to ensuring a consistent base structure.
///
/// Use this repository interface to directly managed entities.
/// </summary>
/// <remarks>
/// This interface provides methods to **track changes** in the database context,
/// but does **not persist them immediately**. Persistence is handled separately
/// through <see cref="IUnitOfWork"/>.
/// </remarks>
/// <typeparam name="TClass">
/// The type that this repository handles.
/// </typeparam>
public interface IEntityRepository<TClass> : IRepository
{
    /// <summary>
    /// Retrieves a **Tracked** single entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>The entity matching the given ID, or <c>null</c> if no entity is found.</returns>
    Task<TClass?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new entity to the database context.
    ///
    /// The entity is **not immediately saved**—it is tracked by the context and
    /// will be persisted through <see cref="IUnitOfWork"/>.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    public void Add(TClass entity);

    /// <summary>
    /// Marks an entity for deletion in the database context.
    ///
    /// The entity is **not immediately removed** from the database—it will be
    /// deleted through <see cref="IUnitOfWork"/>.
    /// </summary>
    /// <param name="entity">The entity to be removed.</param>
    public void Remove(TClass entity);
}
