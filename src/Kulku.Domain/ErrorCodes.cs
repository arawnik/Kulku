namespace Kulku.Domain;

/// <summary>
/// Represents the type of error that occurred.
/// </summary>
public static class ErrorCodes
{
    public const string General = "General";
    public const string NotFound = "NotFound";
    public const string Validation = "Validation"; // For form validation errors (per field)
    public const string BusinessRule = "BusinessRule"; // Businessrule violations, invalid state
    public const string InvalidData = "InvalidData"; // For data processing errors (e.g. database)
    public const string Unauthorized = "Unauthorized";
    public const string Conflict = "Conflict";
}
