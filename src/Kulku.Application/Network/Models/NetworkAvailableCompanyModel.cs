namespace Kulku.Application.Network.Models;

/// <summary>
/// Read model for a company that is available (not yet enrolled) for network tracking.
/// </summary>
public sealed record NetworkAvailableCompanyModel(
    Guid CompanyId,
    string Name,
    string? Website,
    string? Region
);
