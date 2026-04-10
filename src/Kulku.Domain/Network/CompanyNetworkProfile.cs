using Kulku.Domain.Cover;

namespace Kulku.Domain.Network;

/// <summary>
/// Extends a <see cref="Company"/> with network tracking data.
/// One optional profile per company — created on "enroll."
/// </summary>
public class CompanyNetworkProfile
{
    /// <summary>
    /// Unique identifier for the network profile.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key referencing the associated <see cref="Company"/>.
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// The current relationship stage with this company.
    /// </summary>
    public CompanyStage Stage { get; set; }

    /// <summary>
    /// Free-text notes about the relationship or context.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Navigation property to the associated company.
    /// </summary>
    public Company Company { get; set; } = null!;

    /// <summary>
    /// Categories assigned to this company's network profile.
    /// </summary>
    public ICollection<CompanyNetworkProfileCategory> CompanyNetworkProfileCategories { get; init; } =
        [];
}
