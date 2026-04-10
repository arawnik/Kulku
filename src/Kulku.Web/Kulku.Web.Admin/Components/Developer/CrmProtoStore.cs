namespace Kulku.Web.Admin.Components.Developer;

public enum InteractionDirection
{
    Inbound,
    Outbound,
}

public enum InteractionChannel
{
    CvContactForm,
    Email,
    Call,
    LinkedIn,
}

/// <summary>
/// "Long-horizon quality" stages. Keep it coarse.
/// </summary>
public enum CompanyStage
{
    Watchlist,
    Relationship,
    Discovery,
    Proposal,
    ActiveDelivery,
    Parked,
}

/// <summary>
/// One configurable category set used across CRM.
/// Example: HealthTech, Gaming, FinTech, Public Sector...
/// </summary>
public sealed record CategoryLite(Guid Id, string Name, string? ColorToken = null);

/// <summary>
/// CRM-specific profile data for a company. Keyed by the Domain Company's Id.
/// Website and Region live on the Domain Company entity.
/// </summary>
public sealed record CrmCompanyProfile(
    Guid CompanyId,
    CompanyStage Stage,
    string? Notes,
    IReadOnlyList<Guid> CategoryIds
);

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1056:URI-like properties should not be strings",
    Justification = "Prototype record; URI validation deferred to persistence layer."
)]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1054:URI-like parameters should not be strings",
    Justification = "Prototype record; URI validation deferred to persistence layer."
)]
public sealed record ContactLite(
    Guid Id,
    Guid? CompanyId,
    string? PersonName,
    string? Email,
    string? Phone,
    string? LinkedInUrl,
    string? Title
);

public sealed record InteractionLite(
    Guid Id,
    Guid CompanyId,
    DateTime Date,
    InteractionDirection Direction,
    InteractionChannel Channel,
    bool IsWarmIntro,
    string? ReferredByName,
    string? ReferredByRelation,
    Guid? ContactId,
    string Summary,
    string? NextAction,
    DateTime? NextActionDue
);

public enum RelationshipHealth
{
    Fresh,
    Cooling,
    Cold,
    NoHistory,
}

/// <summary>
/// Computed summary of a company relationship for the dashboard directory.
/// </summary>
public sealed record RelationshipSummary(
    Guid CompanyId,
    string CompanyName,
    string? Region,
    CompanyStage Stage,
    IReadOnlyList<CategoryLite> Categories,
    string? PrimaryContactName,
    string? PrimaryContactTitle,
    DateTime? FirstInteractionDate,
    DateTime? LastInteractionDate,
    InteractionChannel? LastChannel,
    string? LastSummary,
    int? DaysSinceLastTouch,
    RelationshipHealth Health,
    string? NextAction,
    DateTime? NextActionDue,
    int InteractionCount,
    int ContactCount
);

public sealed class CrmProtoStore
{
    private readonly List<CategoryLite> _categories = [];
    private readonly List<CrmCompanyProfile> _profiles = [];
    private readonly List<ContactLite> _contacts = [];
    private readonly List<InteractionLite> _interactions = [];

    public IReadOnlyList<CategoryLite> Categories => _categories;
    public IReadOnlyList<CrmCompanyProfile> Profiles => _profiles;
    public IReadOnlyList<ContactLite> Contacts => _contacts;
    public IReadOnlyList<InteractionLite> Interactions => _interactions;

    /// <summary>
    /// Whether the CrmService has already seeded demo data.
    /// </summary>
    public bool IsSeeded { get; set; }

    // ===== Categories =====

    public CategoryLite AddCategory(string name, string? colorToken = null)
    {
        var existing = _categories.FirstOrDefault(c =>
            c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
        );
        if (existing is not null)
            return existing;

        var category = new CategoryLite(Guid.NewGuid(), name.Trim(), colorToken);
        _categories.Add(category);
        return category;
    }

    public CategoryLite? GetCategory(Guid id) => _categories.FirstOrDefault(c => c.Id == id);

    public CategoryLite UpdateCategory(Guid id, string name, string? colorToken)
    {
        var idx = _categories.FindIndex(c => c.Id == id);
        if (idx < 0)
            throw new InvalidOperationException($"Category {id} not found.");
        var updated = new CategoryLite(id, name.Trim(), colorToken);
        _categories[idx] = updated;
        return updated;
    }

    public bool RemoveCategory(Guid id) => _categories.RemoveAll(c => c.Id == id) > 0;

    // ===== CRM Profiles =====

    public CrmCompanyProfile AddProfile(
        Guid companyId,
        CompanyStage stage,
        string? notes,
        IReadOnlyList<Guid> categoryIds
    )
    {
        var existing = _profiles.FirstOrDefault(p => p.CompanyId == companyId);
        if (existing is not null)
            return existing;

        var profile = new CrmCompanyProfile(companyId, stage, notes?.Trim(), categoryIds.ToList());
        _profiles.Add(profile);
        return profile;
    }

    public CrmCompanyProfile? GetProfile(Guid companyId) =>
        _profiles.FirstOrDefault(p => p.CompanyId == companyId);

    public CrmCompanyProfile UpdateProfile(
        Guid companyId,
        CompanyStage stage,
        string? notes,
        IReadOnlyList<Guid> categoryIds
    )
    {
        var idx = _profiles.FindIndex(p => p.CompanyId == companyId);
        if (idx < 0)
            throw new InvalidOperationException($"Profile for company {companyId} not found.");
        var updated = new CrmCompanyProfile(companyId, stage, notes?.Trim(), categoryIds.ToList());
        _profiles[idx] = updated;
        return updated;
    }

    public bool RemoveProfile(Guid companyId)
    {
        _interactions.RemoveAll(i => i.CompanyId == companyId);
        _contacts.RemoveAll(c => c.CompanyId == companyId);
        return _profiles.RemoveAll(p => p.CompanyId == companyId) > 0;
    }

    // ===== Contacts =====

    public ContactLite AddContact(
        Guid? companyId,
        string? personName,
        string? email,
        string? phone,
        string? linkedInUrl,
        string? title
    )
    {
        var contact = new ContactLite(
            Guid.NewGuid(),
            companyId,
            personName?.Trim(),
            email?.Trim(),
            phone?.Trim(),
            linkedInUrl?.Trim(),
            title?.Trim()
        );
        _contacts.Add(contact);
        return contact;
    }

    public ContactLite? GetContact(Guid id) => _contacts.FirstOrDefault(c => c.Id == id);

    public IReadOnlyList<ContactLite> GetCompanyContacts(Guid companyId) =>
        [.. _contacts.Where(c => c.CompanyId == companyId)];

    public IReadOnlyList<ContactLite> GetUnaffiliatedContacts() =>
        [.. _contacts.Where(c => c.CompanyId is null)];

    public ContactLite? MoveContact(Guid contactId, Guid? newCompanyId)
    {
        var idx = _contacts.FindIndex(c => c.Id == contactId);
        if (idx < 0)
            return null;
        var updated = _contacts[idx] with { CompanyId = newCompanyId };
        _contacts[idx] = updated;
        return updated;
    }

    public ContactLite UpdateContact(
        Guid id,
        string? personName,
        string? email,
        string? phone,
        string? linkedInUrl,
        string? title
    )
    {
        var idx = _contacts.FindIndex(c => c.Id == id);
        if (idx < 0)
            throw new InvalidOperationException($"Contact {id} not found.");
        var existing = _contacts[idx];
        var updated = existing with
        {
            PersonName = personName?.Trim(),
            Email = email?.Trim(),
            Phone = phone?.Trim(),
            LinkedInUrl = linkedInUrl?.Trim(),
            Title = title?.Trim(),
        };
        _contacts[idx] = updated;
        return updated;
    }

    public bool RemoveContact(Guid id)
    {
        foreach (var interaction in _interactions.Where(i => i.ContactId == id).ToList())
        {
            var idx = _interactions.FindIndex(i => i.Id == interaction.Id);
            if (idx >= 0)
                _interactions[idx] = interaction with { ContactId = null };
        }
        return _contacts.RemoveAll(c => c.Id == id) > 0;
    }

    // ===== Interactions =====

    public void AddInteraction(InteractionLite interaction) => _interactions.Add(interaction);

    public InteractionLite? GetInteraction(Guid id) =>
        _interactions.FirstOrDefault(i => i.Id == id);

    public IReadOnlyList<InteractionLite> GetCompanyInteractions(Guid companyId) =>
        _interactions.Where(i => i.CompanyId == companyId).OrderByDescending(i => i.Date).ToList();

    public InteractionLite UpdateInteraction(
        Guid id,
        DateTime date,
        InteractionDirection direction,
        InteractionChannel channel,
        bool isWarmIntro,
        string? referredByName,
        string? referredByRelation,
        Guid? contactId,
        string summary,
        string? nextAction,
        DateTime? nextActionDue
    )
    {
        var idx = _interactions.FindIndex(i => i.Id == id);
        if (idx < 0)
            throw new InvalidOperationException($"Interaction {id} not found.");
        var existing = _interactions[idx];
        var updated = existing with
        {
            Date = date,
            Direction = direction,
            Channel = channel,
            IsWarmIntro = isWarmIntro,
            ReferredByName = referredByName,
            ReferredByRelation = referredByRelation,
            ContactId = contactId,
            Summary = summary.Trim(),
            NextAction = string.IsNullOrWhiteSpace(nextAction) ? null : nextAction.Trim(),
            NextActionDue = nextActionDue,
        };
        _interactions[idx] = updated;
        return updated;
    }

    public bool RemoveInteraction(Guid id) => _interactions.RemoveAll(i => i.Id == id) > 0;
}
