namespace Kulku.Domain.Contacts;

/// <summary>
/// Tracks the lifecycle status of a user-submitted contact request.
/// </summary>
public enum ContactRequestStatus
{
    /// <summary>The request has been received but not yet reviewed.</summary>
    New,

    /// <summary>The request has been converted into a network interaction.</summary>
    Converted,

    /// <summary>The request was identified as spam.</summary>
    Spam,

    /// <summary>The request was reviewed and dismissed without action.</summary>
    Dismissed,
}
