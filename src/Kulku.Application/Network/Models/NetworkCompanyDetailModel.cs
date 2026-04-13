using Kulku.Domain.Network;

namespace Kulku.Application.Network.Models;

/// <summary>
/// Detailed read model for a single enrolled company, including latest interaction metadata.
/// </summary>
public sealed record NetworkCompanyDetailModel(
    Guid CompanyId,
    string Name,
    string? Website,
    string? Region,
    CompanyStage Stage,
    string? Notes,
    IReadOnlyList<NetworkCategoryModel> Categories,
    int ContactCount,
    int InteractionCount,
    DateTime? LatestInteractionDate,
    string? PrimaryContactName
);
