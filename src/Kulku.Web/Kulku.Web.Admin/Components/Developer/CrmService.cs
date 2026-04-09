using Kulku.Application.Cover.Company;
using Kulku.Application.Cover.Company.Models;
using Kulku.Application.Cover.Models;
using Kulku.Domain;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace Kulku.Web.Admin.Components.Developer;

/// <summary>
/// Combined view model merging Domain Company identity with CRM profile data.
/// </summary>
public sealed record CrmCompanyViewModel(
	Guid Id,
	string Name,
	string? Description,
	string? Website,
	string? Region,
	CrmCompanyProfile? Profile
);

/// <summary>
/// Scoped service that joins Domain Company data (from DB) with CRM profile data
/// (from the in-memory <see cref="CrmProtoStore"/>).
/// </summary>
public sealed class CrmService(
	CrmProtoStore store,
	IQueryHandler<GetCompanies.Query, IReadOnlyList<CompanyTranslationsModel>> companiesHandler,
	ICommandHandler<CreateCompany.Command, Guid> createCompanyHandler
)
{
	private readonly CrmProtoStore _store = store;
	private readonly IQueryHandler<GetCompanies.Query, IReadOnlyList<CompanyTranslationsModel>> _companiesHandler = companiesHandler;
	private readonly ICommandHandler<CreateCompany.Command, Guid> _createCompanyHandler = createCompanyHandler;

	/// <summary>
	/// Returns companies that have CRM profiles, merged with DB identity.
	/// </summary>
	public async Task<IReadOnlyList<CrmCompanyViewModel>> GetEnrolledCompaniesAsync(
		CancellationToken ct = default)
	{
		await EnsureSeededAsync(ct);
		var dbCompanies = await GetDbCompaniesAsync(ct);
		var profileIds = _store.Profiles.Select(p => p.CompanyId).ToHashSet();

		return dbCompanies
			.Where(c => profileIds.Contains(c.CompanyId))
			.Select(c => ToViewModel(c, _store.GetProfile(c.CompanyId)))
			.ToList();
	}

	/// <summary>
	/// Returns a single company with full CRM + CV data.
	/// </summary>
	public async Task<CrmCompanyViewModel?> GetCompanyDetailAsync(
		Guid companyId, CancellationToken ct = default)
	{
		await EnsureSeededAsync(ct);
		var dbCompanies = await GetDbCompaniesAsync(ct);
		var dbCompany = dbCompanies.FirstOrDefault(c => c.CompanyId == companyId);
		if (dbCompany is null) return null;

		return ToViewModel(dbCompany, _store.GetProfile(companyId));
	}

	/// <summary>
	/// Returns CV companies that are NOT yet enrolled in CRM.
	/// </summary>
	public async Task<IReadOnlyList<CrmCompanyViewModel>> GetAvailableCompaniesAsync(
		CancellationToken ct = default)
	{
		await EnsureSeededAsync(ct);
		var dbCompanies = await GetDbCompaniesAsync(ct);
		var profileIds = _store.Profiles.Select(p => p.CompanyId).ToHashSet();

		return dbCompanies
			.Where(c => !profileIds.Contains(c.CompanyId))
			.Select(c => ToViewModel(c, null))
			.ToList();
	}

	/// <summary>
	/// Enrolls an existing Domain Company into CRM tracking.
	/// </summary>
	public CrmCompanyProfile EnrollCompany(Guid companyId, CompanyStage stage,
		string? notes, IReadOnlyList<Guid> categoryIds) =>
		_store.AddProfile(companyId, stage, notes, categoryIds);

	/// <summary>
	/// Creates a new Domain Company (with minimal translations) and immediately enrolls it in CRM.
	/// </summary>
	public async Task<CrmCompanyViewModel?> CreateAndEnrollAsync(
		string name, string? website, string? region,
		CompanyStage stage, string? notes, IReadOnlyList<Guid> categoryIds,
		CancellationToken ct = default)
	{
		var translations = Defaults.SupportedCultures
			.Select(LanguageCodeFromCulture)
			.Where(lc => lc.HasValue)
			.Select(lc => new CompanyTranslationDto(lc!.Value, name, string.Empty))
			.ToList();

		var result = await _createCompanyHandler.Handle(
			new CreateCompany.Command(website, region, translations), ct);

		if (!result.IsSuccess) return null;

		var companyId = result.Value;
		var profile = _store.AddProfile(companyId, stage, notes, categoryIds);

		return new CrmCompanyViewModel(companyId, name, null, website, region, profile);
	}

	/// <summary>
	/// Gets a company name by Id from the DB (for display in interaction streams, etc.).
	/// </summary>
	public async Task<string> GetCompanyNameAsync(Guid companyId, CancellationToken ct = default)
	{
		var dbCompanies = await GetDbCompaniesAsync(ct);
		var company = dbCompanies.FirstOrDefault(c => c.CompanyId == companyId);
		if (company is null) return "Unknown";
		var enTrans = company.Translations.FirstOrDefault(t => t.Language == Kulku.Domain.LanguageCode.English);
		return enTrans?.Name ?? (company.Translations.Count > 0 ? company.Translations[0].Name : null) ?? "Unknown";
	}

	/// <summary>
	/// Seeds demo CRM profiles/contacts/interactions for existing CV companies on first access.
	/// </summary>
	public async Task EnsureSeededAsync(CancellationToken ct = default)
	{
		if (_store.IsSeeded) return;
		_store.IsSeeded = true;

		var dbCompanies = await GetDbCompaniesAsync(ct);
		if (dbCompanies.Count == 0) return;

		// Ensure categories exist
		var health = _store.AddCategory("Health Tech", "success");
		_store.AddCategory("Gaming", "warning");
		var fintech = _store.AddCategory("FinTech", "primary");
		var b2b = _store.AddCategory("B2B SaaS", "info");
		var publicSector = _store.AddCategory("Public Sector", "secondary");

		// Enroll up to 4 companies with demo CRM profiles
		var stages = new[]
		{
			(CompanyStage.Relationship, "Long-term platform modernization potential.", new[] { fintech.Id }),
			(CompanyStage.Discovery, "Data pipelines + cloud cost optimization; likely recurring needs.", new[] { b2b.Id }),
			(CompanyStage.Proposal, "Product studio; could become a repeat delivery partner.", new[] { health.Id }),
			(CompanyStage.Parked, "Procurement cycles; long horizon. Keep warm without spamming.", new[] { publicSector.Id }),
		};

		var enrolledCompanyIds = new List<Guid>();

		for (var i = 0; i < Math.Min(dbCompanies.Count, stages.Length); i++)
		{
			var (stage, notes, catIds) = stages[i];
			var companyId = dbCompanies[i].CompanyId;
			_store.AddProfile(companyId, stage, notes, catIds);
			enrolledCompanyIds.Add(companyId);
		}

		if (enrolledCompanyIds.Count == 0) return;

		// Add demo contacts for enrolled companies
		var contactIds = new List<Guid>();
		string[][] contactData =
		[
			["CTO", "cto@example.com", "", "", "CTO"],
			["Head of Data", "", "", "https://linkedin.com/in/example", "Head of Data"],
			["Product Lead", "pl@example.com", "", "", "Product Lead"],
			["Procurement", "proc@example.com", "", "", "Procurement"],
		];

		for (var i = 0; i < Math.Min(enrolledCompanyIds.Count, contactData.Length); i++)
		{
			var d = contactData[i];
			var c = _store.AddContact(enrolledCompanyIds[i],
				NullIfEmpty(d[0]), NullIfEmpty(d[1]), NullIfEmpty(d[2]), NullIfEmpty(d[3]), NullIfEmpty(d[4]));
			contactIds.Add(c.Id);
		}

		// Add demo interactions
		if (enrolledCompanyIds.Count >= 1 && contactIds.Count >= 1)
		{
			_store.AddInteraction(new InteractionLite(Guid.NewGuid(), enrolledCompanyIds[0],
				DateTime.Today.AddDays(-10), InteractionDirection.Inbound, InteractionChannel.CvContactForm,
				true, "Mikko (ex-colleague)", "Colleague", contactIds[0],
				"Inbound via CV contact form; wants modernization workshop; timeline Q2\u2013Q3.",
				"Send 1-pager + propose 60min workshop slot", DateTime.Today.AddDays(7)));

			_store.AddInteraction(new InteractionLite(Guid.NewGuid(), enrolledCompanyIds[0],
				DateTime.Today.AddDays(-7), InteractionDirection.Outbound, InteractionChannel.Email,
				false, null, null, contactIds[0],
				"Sent short follow-up email with workshop agenda + 2 relevant references.",
				"Confirm workshop slot", DateTime.Today.AddDays(6)));
		}

		if (enrolledCompanyIds.Count >= 2 && contactIds.Count >= 2)
		{
			_store.AddInteraction(new InteractionLite(Guid.NewGuid(), enrolledCompanyIds[1],
				DateTime.Today.AddDays(-24), InteractionDirection.Inbound, InteractionChannel.LinkedIn,
				false, null, null, contactIds[1],
				"LinkedIn message: needs help with data platform reliability & cost.",
				"Reply with 2 relevant references + suggest intro call", DateTime.Today.AddDays(5)));

			_store.AddInteraction(new InteractionLite(Guid.NewGuid(), enrolledCompanyIds[1],
				DateTime.Today.AddDays(-20), InteractionDirection.Outbound, InteractionChannel.LinkedIn,
				false, null, null, contactIds[1],
				"Replied with brief note + asked for 30min discovery call; kept it tight and relevant.",
				null, null));
		}

		if (enrolledCompanyIds.Count >= 3 && contactIds.Count >= 3)
		{
			_store.AddInteraction(new InteractionLite(Guid.NewGuid(), enrolledCompanyIds[2],
				DateTime.Today.AddDays(-4), InteractionDirection.Inbound, InteractionChannel.Email,
				true, "Anna (friend)", "Friend", contactIds[2],
				"Warm intro via friend; exploring delivery support for upcoming release cycle.",
				"Schedule discovery call + ask for constraints & priorities", DateTime.Today.AddDays(3)));
		}

		if (enrolledCompanyIds.Count >= 4 && contactIds.Count >= 4)
		{
			_store.AddInteraction(new InteractionLite(Guid.NewGuid(), enrolledCompanyIds[3],
				DateTime.Today.AddDays(-120), InteractionDirection.Inbound, InteractionChannel.Email,
				false, null, null, contactIds[3],
				"Older tender cycle ended; keep an eye on next procurement window.",
				"Re-check procurement calendar", DateTime.Today.AddDays(45)));
		}
	}

	private async Task<IReadOnlyList<CompanyTranslationsModel>> GetDbCompaniesAsync(CancellationToken ct)
	{
		var result = await _companiesHandler.Handle(new GetCompanies.Query(), ct);
		return result.IsSuccess ? result.Value ?? [] : [];
	}

	private static CrmCompanyViewModel ToViewModel(CompanyTranslationsModel db, CrmCompanyProfile? profile)
	{
		// Pick English name, fall back to first available
		var enTrans = db.Translations.FirstOrDefault(t => t.Language == Kulku.Domain.LanguageCode.English);
		var fallback = db.Translations.Count > 0 ? db.Translations[0] : null;
		var name = enTrans?.Name ?? fallback?.Name ?? "Unknown";
		var description = enTrans?.Description ?? fallback?.Description;

		return new CrmCompanyViewModel(db.CompanyId, name, description, db.Website, db.Region, profile);
	}

	private static string? NullIfEmpty(string s) =>
		string.IsNullOrWhiteSpace(s) ? null : s;

	private static LanguageCode? LanguageCodeFromCulture(string culture) =>
		culture switch
		{
			"en" => LanguageCode.English,
			"fi" => LanguageCode.Finnish,
			_ => null,
		};
}
