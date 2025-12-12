using Microsoft.AspNetCore.Identity;

namespace Kulku.Persistence;

/// <summary>
/// Represents an application user in the system. Only exists in Infrastructure, because it's not in our domain.
/// Inherits from <see cref="IdentityUser"/> to provide authentication and authorization features.
/// </summary>
public class ApplicationUser : IdentityUser;
