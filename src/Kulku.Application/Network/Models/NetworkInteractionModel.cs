using Kulku.Domain.Network;

namespace Kulku.Application.Network.Models;

/// <summary>
/// Read model for a network interaction.
/// </summary>
public sealed record NetworkInteractionModel(
    Guid Id,
    Guid CompanyId,
    string CompanyName,
    Guid? ContactId,
    string? ContactName,
    DateTime Date,
    InteractionDirection Direction,
    InteractionChannel Channel,
    bool IsWarmIntro,
    string? ReferredByName,
    string? ReferredByRelation,
    string Summary,
    string? NextAction,
    DateTime? NextActionDue,
    DateTime CreatedAt
);
