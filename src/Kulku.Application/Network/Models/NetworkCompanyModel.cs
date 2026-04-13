using Kulku.Domain.Network;

namespace Kulku.Application.Network.Models;

/// <summary>
/// Read model for a company enrolled in the professional network.
/// </summary>
public sealed record NetworkCompanyModel(
    Guid CompanyId,
    string Name,
    string? Website,
    string? Region,
    CompanyStage Stage,
    string? Notes,
    IReadOnlyList<NetworkCategoryModel> Categories,
    int ContactCount,
    int InteractionCount
);
