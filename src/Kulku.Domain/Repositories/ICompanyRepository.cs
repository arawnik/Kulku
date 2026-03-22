using Kulku.Domain.Abstractions;
using Kulku.Domain.Cover;

namespace Kulku.Domain.Repositories;

/// <summary>
/// Repository for managing <see cref="Company"/> entities.
/// </summary>
public interface ICompanyRepository : IEntityRepository<Company>;
