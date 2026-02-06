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

public enum IdeaStatus
{
    Seed,
    Exploring,
    Validating,
    Parked,
    PromotedToBacklog,
}

/// <summary>
/// One configurable category set used across CRM + IdeaBank.
/// Example: HealthTech, Gaming, FinTech, Public Sector...
/// </summary>
public sealed record CategoryLite(Guid Id, string Name, string? ColorToken = null);

public sealed record CompanyLite(
    Guid Id,
    string Name,
    string? Website,
    string? Region,
    string? Notes,
    CompanyStage Stage,
    IReadOnlyList<Guid> CategoryIds
);

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1056:URI-like properties should not be strings",
    Justification = "<Pending>"
)]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1054:URI-like parameters should not be strings",
    Justification = "<Pending>"
)]
public sealed record ContactLite(
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
    string? ReferredByRelation, // Friend/Colleague/Other
    ContactLite Contact,
    string Summary,
    string? NextAction,
    DateTime? NextActionDue
);

public sealed record IdeaLite(
    Guid Id,
    string Title,
    string? OneLiner,
    string Description,
    IdeaStatus Status,
    int ValueScore, // 1–10
    int FeasibilityScore, // 1–10
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<Guid> CategoryIds,
    IReadOnlyList<Guid> LinkedCompanyIds
);

public sealed class CrmProtoStore
{
    private readonly List<CategoryLite> _categories = new();
    private readonly List<CompanyLite> _companies = new();
    private readonly List<InteractionLite> _interactions = new();
    private readonly List<IdeaLite> _ideas = new();

    public IReadOnlyList<CategoryLite> Categories => _categories;
    public IReadOnlyList<CompanyLite> Companies => _companies;
    public IReadOnlyList<InteractionLite> Interactions => _interactions;
    public IReadOnlyList<IdeaLite> Ideas => _ideas;

    public CrmProtoStore()
    {
        // Categories (configurable later)
        var health = AddCategory("Health Tech", "success");
        var gaming = AddCategory("Gaming", "warning");
        var fintech = AddCategory("FinTech", "primary");
        var b2b = AddCategory("B2B SaaS", "info");
        var publicSector = AddCategory("Public Sector", "secondary");

        // Companies (stage + categories)
        var c1 = AddCompany(
            name: "Nordic FinTech Group",
            website: "https://example.com",
            region: "Helsinki",
            notes: "Long-term platform modernization potential.",
            stage: CompanyStage.Relationship,
            categoryIds: [fintech.Id]
        );

        var c2 = AddCompany(
            name: "Industrial Analytics Oy",
            website: null,
            region: "Tampere",
            notes: "Data pipelines + cloud cost optimization; likely recurring needs.",
            stage: CompanyStage.Discovery,
            categoryIds: [b2b.Id]
        );

        var c3 = AddCompany(
            name: "Health Product Studio",
            website: null,
            region: "Remote EU",
            notes: "Product studio; could become a repeat delivery partner.",
            stage: CompanyStage.Proposal,
            categoryIds: [health.Id]
        );

        var c4 = AddCompany(
            name: "Public Sector Digital Unit",
            website: null,
            region: "Finland",
            notes: "Procurement cycles; long horizon. Keep warm without spamming.",
            stage: CompanyStage.Parked,
            categoryIds: [publicSector.Id]
        );

        // Interactions (include inbound + outbound)
        AddInteraction(
            new InteractionLite(
                Id: Guid.NewGuid(),
                CompanyId: c1.Id,
                Date: DateTime.Today.AddDays(-10),
                Direction: InteractionDirection.Inbound,
                Channel: InteractionChannel.CvContactForm,
                IsWarmIntro: true,
                ReferredByName: "Mikko (ex-colleague)",
                ReferredByRelation: "Colleague",
                Contact: new ContactLite("CTO", "cto@example.com", null, null, "CTO"),
                Summary: "Inbound via CV contact form; wants modernization workshop; timeline Q2–Q3.",
                NextAction: "Send 1-pager + propose 60min workshop slot",
                NextActionDue: DateTime.Today.AddDays(7)
            )
        );

        AddInteraction(
            new InteractionLite(
                Id: Guid.NewGuid(),
                CompanyId: c1.Id,
                Date: DateTime.Today.AddDays(-7),
                Direction: InteractionDirection.Outbound,
                Channel: InteractionChannel.Email,
                IsWarmIntro: false,
                ReferredByName: null,
                ReferredByRelation: null,
                Contact: new ContactLite("CTO", "cto@example.com", null, null, "CTO"),
                Summary: "Sent short follow-up email with workshop agenda + 2 relevant references.",
                NextAction: "Confirm workshop slot",
                NextActionDue: DateTime.Today.AddDays(6)
            )
        );

        AddInteraction(
            new InteractionLite(
                Id: Guid.NewGuid(),
                CompanyId: c2.Id,
                Date: DateTime.Today.AddDays(-24),
                Direction: InteractionDirection.Inbound,
                Channel: InteractionChannel.LinkedIn,
                IsWarmIntro: false,
                ReferredByName: null,
                ReferredByRelation: null,
                Contact: new ContactLite(
                    "Head of Data",
                    null,
                    null,
                    "https://linkedin.com/in/example",
                    "Head of Data"
                ),
                Summary: "LinkedIn message: needs help with data platform reliability & cost.",
                NextAction: "Reply with 2 relevant references + suggest intro call",
                NextActionDue: DateTime.Today.AddDays(5)
            )
        );

        AddInteraction(
            new InteractionLite(
                Id: Guid.NewGuid(),
                CompanyId: c2.Id,
                Date: DateTime.Today.AddDays(-20),
                Direction: InteractionDirection.Outbound,
                Channel: InteractionChannel.LinkedIn,
                IsWarmIntro: false,
                ReferredByName: null,
                ReferredByRelation: null,
                Contact: new ContactLite(
                    "Head of Data",
                    null,
                    null,
                    "https://linkedin.com/in/example",
                    "Head of Data"
                ),
                Summary: "Replied with brief note + asked for 30min discovery call; kept it tight and relevant.",
                NextAction: null,
                NextActionDue: null
            )
        );

        AddInteraction(
            new InteractionLite(
                Id: Guid.NewGuid(),
                CompanyId: c3.Id,
                Date: DateTime.Today.AddDays(-4),
                Direction: InteractionDirection.Inbound,
                Channel: InteractionChannel.Email,
                IsWarmIntro: true,
                ReferredByName: "Anna (friend)",
                ReferredByRelation: "Friend",
                Contact: new ContactLite(
                    "Product Lead",
                    "pl@example.com",
                    null,
                    null,
                    "Product Lead"
                ),
                Summary: "Warm intro via friend; exploring delivery support for upcoming release cycle.",
                NextAction: "Schedule discovery call + ask for constraints & priorities",
                NextActionDue: DateTime.Today.AddDays(3)
            )
        );

        AddInteraction(
            new InteractionLite(
                Id: Guid.NewGuid(),
                CompanyId: c4.Id,
                Date: DateTime.Today.AddDays(-120),
                Direction: InteractionDirection.Inbound,
                Channel: InteractionChannel.Email,
                IsWarmIntro: false,
                ReferredByName: null,
                ReferredByRelation: null,
                Contact: new ContactLite(
                    "Procurement",
                    "proc@example.com",
                    null,
                    null,
                    "Procurement"
                ),
                Summary: "Older tender cycle ended; keep an eye on next procurement window.",
                NextAction: "Re-check procurement calendar",
                NextActionDue: DateTime.Today.AddDays(45)
            )
        );

        // Ideas (linkable to companies + categorized)
        AddOrUpdateIdea(
            new IdeaLite(
                Id: Guid.NewGuid(),
                Title: "Secure inbound funnel → CRM pipeline",
                OneLiner: "Make CV contact + email capture into unified interaction logging.",
                Description: "Unify contact form submissions, email, and LinkedIn into one Interaction model. Add basic spam protection and a review queue. Promote validated opportunities into CRM focus list.",
                Status: IdeaStatus.Validating,
                ValueScore: 9,
                FeasibilityScore: 8,
                CreatedAt: DateTime.Today.AddDays(-30),
                UpdatedAt: DateTime.Today.AddDays(-6),
                CategoryIds: [b2b.Id, fintech.Id],
                LinkedCompanyIds: [c1.Id, c2.Id]
            )
        );

        AddOrUpdateIdea(
            new IdeaLite(
                Id: Guid.NewGuid(),
                Title: "Long-horizon relationship timeline view",
                OneLiner: "Timeline grouped by quarter with 'staleness' decisions.",
                Description: "Company page shows touchpoints grouped by quarter; prompts to decide: rekindle / park / archive. Prevents 'inbox CRM' behavior.",
                Status: IdeaStatus.Exploring,
                ValueScore: 8,
                FeasibilityScore: 7,
                CreatedAt: DateTime.Today.AddDays(-18),
                UpdatedAt: DateTime.Today.AddDays(-18),
                CategoryIds: [publicSector.Id],
                LinkedCompanyIds: [c4.Id]
            )
        );

        AddOrUpdateIdea(
            new IdeaLite(
                Id: Guid.NewGuid(),
                Title: "Domain-driven 'case study generator'",
                OneLiner: "Convert internal notes into polished CV/portfolio artifacts.",
                Description: "Take CRM interactions + delivery notes and generate anonymized case studies to publish. Drives inbound quality long-term.",
                Status: IdeaStatus.Seed,
                ValueScore: 7,
                FeasibilityScore: 5,
                CreatedAt: DateTime.Today.AddDays(-5),
                UpdatedAt: DateTime.Today.AddDays(-5),
                CategoryIds: [health.Id, gaming.Id],
                LinkedCompanyIds: [c3.Id]
            )
        );
    }

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

    // ===== Companies =====
    public CompanyLite AddCompany(
        string name,
        string? website,
        string? region,
        string? notes,
        CompanyStage stage,
        IReadOnlyList<Guid> categoryIds
    )
    {
        var existing = _companies.FirstOrDefault(c =>
            c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
        );
        if (existing is not null)
            return existing;

        var company = new CompanyLite(
            Id: Guid.NewGuid(),
            Name: name.Trim(),
            Website: website?.Trim(),
            Region: region?.Trim(),
            Notes: notes?.Trim(),
            Stage: stage,
            CategoryIds: categoryIds.ToList()
        );

        _companies.Add(company);
        return company;
    }

    public CompanyLite? GetCompany(Guid id) => _companies.FirstOrDefault(c => c.Id == id);

    public CompanyLite UpdateCompanyCategories(Guid companyId, IReadOnlyList<Guid> categoryIds)
    {
        var company = _companies.First(c => c.Id == companyId);
        var updated = company with { CategoryIds = categoryIds.ToList() };
        _companies[_companies.IndexOf(company)] = updated;
        return updated;
    }

    // ===== Interactions =====
    public void AddInteraction(InteractionLite interaction) => _interactions.Add(interaction);

    public IReadOnlyList<InteractionLite> GetCompanyInteractions(Guid companyId) =>
        _interactions.Where(i => i.CompanyId == companyId).OrderByDescending(i => i.Date).ToList();

    // ===== Ideas =====
    public IdeaLite? GetIdea(Guid id) => _ideas.FirstOrDefault(i => i.Id == id);

    public void AddOrUpdateIdea(IdeaLite idea)
    {
        var existing = _ideas.FirstOrDefault(i => i.Id == idea.Id);
        if (existing is null)
        {
            _ideas.Add(idea);
            return;
        }

        _ideas[_ideas.IndexOf(existing)] = idea;
    }

    public void DeleteIdea(Guid id)
    {
        var existing = _ideas.FirstOrDefault(i => i.Id == id);
        if (existing is not null)
        {
            _ideas.Remove(existing);
        }
    }
}
