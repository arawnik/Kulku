namespace Kulku.Domain.Network;

/// <summary>
/// Join entity for the many-to-many relationship between
/// <see cref="CompanyNetworkProfile"/> and <see cref="NetworkCategory"/>.
/// </summary>
public class CompanyNetworkProfileCategory
{
    /// <summary>
    /// Foreign key to the network profile.
    /// </summary>
    public Guid CompanyNetworkProfileId { get; set; }

    /// <summary>
    /// Foreign key to the category.
    /// </summary>
    public Guid NetworkCategoryId { get; set; }

    /// <summary>
    /// Navigation property to the network profile.
    /// </summary>
    public CompanyNetworkProfile CompanyNetworkProfile { get; set; } = null!;

    /// <summary>
    /// Navigation property to the category.
    /// </summary>
    public NetworkCategory NetworkCategory { get; set; } = null!;
}
