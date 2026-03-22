using Kulku.Domain.Abstractions;
using Kulku.Domain.Contacts;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="ContactRequest"/> entities.
/// </summary>
public interface IContactRequestRepository : IEntityRepository<ContactRequest>;
