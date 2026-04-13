using Kulku.Domain.Cover;

namespace Kulku.Domain.Network;

/// <summary>
/// Records a single interaction (touchpoint) with a company in the professional network.
/// </summary>
public class NetworkInteraction
{
    /// <summary>
    /// Unique identifier for the interaction.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key referencing the company this interaction is logged against.
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Optional foreign key to the specific contact involved.
    /// </summary>
    public Guid? ContactId { get; set; }

    /// <summary>
    /// The date of the interaction.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Whether the interaction was inbound or outbound.
    /// </summary>
    public InteractionDirection Direction { get; set; }

    /// <summary>
    /// The communication channel used.
    /// </summary>
    public InteractionChannel Channel { get; set; }

    /// <summary>
    /// Whether this interaction came through a warm introduction.
    /// </summary>
    public bool IsWarmIntro { get; set; }

    /// <summary>
    /// Name of the person who referred this interaction, if a warm intro.
    /// </summary>
    public string? ReferredByName { get; set; }

    /// <summary>
    /// Relationship to the referrer (e.g. "Colleague", "Friend").
    /// </summary>
    public string? ReferredByRelation { get; set; }

    /// <summary>
    /// Summary of what was discussed or happened.
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Planned next action, if any.
    /// </summary>
    public string? NextAction { get; set; }

    /// <summary>
    /// Due date for the next action.
    /// </summary>
    public DateTime? NextActionDue { get; set; }

    /// <summary>
    /// UTC timestamp when this interaction was recorded.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property to the company.
    /// </summary>
    public Company Company { get; set; } = null!;

    /// <summary>
    /// Navigation property to the contact, if specified.
    /// </summary>
    public NetworkContact? Contact { get; set; }
}
