namespace Kulku.Domain.Network;

/// <summary>
/// A user-managed label for categorizing network companies
/// (e.g. "HealthTech", "FinTech", "Public Sector").
/// </summary>
public class NetworkCategory
{
    /// <summary>
    /// Unique identifier for the category.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display name of the category.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional Bootstrap color token for UI display (e.g. "success", "primary").
    /// </summary>
    public string? ColorToken { get; set; }

    /// <summary>
    /// Profiles assigned to this category.
    /// </summary>
    public ICollection<CompanyNetworkProfileCategory> CompanyNetworkProfileCategories { get; init; } =
        [];
}
