namespace Kulku.Domain.Contacts;

/// <summary>
/// Represents a user-submitted contact request, typically sent via a public-facing form.
/// </summary>
public class ContactRequest
{
    /// <summary>
    /// The unique identifier for the contact request.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the person submitting the request.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The email address provided by the sender.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The subject line of the message.
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    /// The message content provided in the contact request.
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    /// The UTC timestamp when the contact request was created.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
