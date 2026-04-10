using Kulku.Domain.Cover;

namespace Kulku.Domain.Network;

/// <summary>
/// Represents a person in the professional network, optionally affiliated with a company.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1056:URI-like properties should not be strings",
    Justification = "Domain entity; URI validation handled at the application edge."
)]
public class NetworkContact
{
    /// <summary>
    /// Unique identifier for the contact.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Optional foreign key to the associated <see cref="Company"/>.
    /// Null for personal (unaffiliated) contacts.
    /// </summary>
    public Guid? CompanyId { get; set; }

    /// <summary>
    /// The contact's name.
    /// </summary>
    public string? PersonName { get; set; }

    /// <summary>
    /// The contact's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// The contact's phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// The contact's LinkedIn profile URL.
    /// </summary>
    public string? LinkedInUrl { get; set; }

    /// <summary>
    /// The contact's professional title or role.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// UTC timestamp when this contact was added.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property to the associated company, if any.
    /// </summary>
    public Company? Company { get; set; }
}
