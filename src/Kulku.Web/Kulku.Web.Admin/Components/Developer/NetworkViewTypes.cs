using Kulku.Application.Network.Models;
using Kulku.Domain.Network;

namespace Kulku.Web.Admin.Components.Developer;

/// <summary>
/// Freshness of a company relationship based on last interaction date.
/// Computed in the dashboard code-behind — not persisted.
/// </summary>
public enum RelationshipHealth
{
    Fresh,
    Cooling,
    Cold,
    NoHistory,
}

/// <summary>
/// Computed summary of a company relationship for the dashboard directory.
/// Built from <see cref="NetworkCompanyModel"/> + interaction/contact queries.
/// </summary>
public sealed record RelationshipSummary(
    Guid CompanyId,
    string CompanyName,
    string? Region,
    CompanyStage Stage,
    IReadOnlyList<NetworkCategoryModel> Categories,
    string? PrimaryContactName,
    string? PrimaryContactTitle,
    DateTime? FirstInteractionDate,
    DateTime? LastInteractionDate,
    InteractionChannel? LastChannel,
    string? LastSummary,
    int? DaysSinceLastTouch,
    RelationshipHealth Health,
    string? NextAction,
    DateTime? NextActionDue,
    int InteractionCount,
    int ContactCount
);
