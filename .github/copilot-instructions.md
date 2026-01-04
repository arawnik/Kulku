# Copilot Instructions — Kulku (Public Showcase Architecture)

This repository is a public-facing showcase. Optimize for coherence, readability, and architectural correctness over “quick hacks”.

## Core architectural principles

- **Clean Architecture dependency direction**
  - `Domain` has no dependencies.
  - `Application` depends on `Domain` only (and library abstractions). Never on `Infrastructure`, `Persistence`, or any presentation layer project.
  - `Infrastructure` and `Persistence` implement ports defined by `Application`.
  - `Web.Api` is a presentation layer. It references `Application` and `Domain` and wires DI.
  - Avoid outer-layer-to-outer-layer dependencies.

- **CQRS**
  - Reads (queries) are implemented via query ports (e.g., `IProjectQueries`) and query handlers.
  - Writes (commands) are implemented via command handlers and domain repositories/stores.
  - Avoid “repository returns DTO” patterns. Prefer read ports for read models.

- **Vertical slice pragmatism**
  - Keep use cases compact and feature-scoped.
  - Use a feature folder (`Application/Projects`) with:
    - use cases at the root (or per-use-case subfolders if they grow),
    - reusable feature response models under `Models/`,
    - ports under `Ports/` (or at feature root if only one port).
  - Avoid over-sharding into dozens of microfiles unless the slice grows.

## Naming and folder conventions

- **Avoid “junk drawer” namespaces/folders**:
  - Do NOT introduce `Helpers`, `Shared`, `Common`, or `Utils` unless there is a very strong reason.
  - Use intent-driven folders/names:
    - `Application.Abstractions.Localization`, `Application.Abstractions.Security`, `Web.Api.Http`, `Web.Api.Localization`.

- **Use-case naming**
  - Use-case classes are named by intent: `GetProjects`, `GetKeywords`, `CreateContactRequest`, etc.
  - Method names on read ports should be intent-based and minimal:
    - Prefer `ListAsync(...)`, `FindByIdAsync(...)`, `ListByTypeAsync(...)` over `QueryAllAsync(...)` or any CRUD-like naming.

- **Data shape naming**
  - Application response shapes use `*Model` (e.g., `ProjectModel`, `KeywordModel`).
  - Application request shapes use `*Dto` (e.g., `ContactRequestDto`).
  - Avoid `*Response` unless it is a true transport/HTTP response DTO.
  - Avoid `*Request` unless it is a true transport/HTTP response DTO.

- **Collections**
  - Prefer `IReadOnlyList<T>` for read-side return types and models.
  - Avoid `ICollection<T>` in read models unless mutation is required.

## Localization and LanguageCode

- `LanguageCode` is a **Domain** enum (persisted in translation tables).
- Language selection is a **presentation concern**:
  - API/UI resolves culture/language and passes it explicitly to queries/commands.
  - Domain/Application/Infrastructure must NOT read ambient culture (`CultureInfo.CurrentCulture`) directly.
- Localized use cases may implement/accept `ILocalizedRequest` and must be explicit about language.
- Mapping from `CultureInfo` to `LanguageCode` belongs in `Application.Abstractions.Localization` (policy) or in `Web.Api.Localization` (edge).

## Enums and persistence

- Domain enums must NOT use `[EnumMember]` for persistence/serialization.
- EF Core persistence policy:
  - Default: store enums as strings (enum member names) via `ConfigureConventions`.
  - Exception: `LanguageCode` uses an explicit `LanguageCodeValueConverter` with defined codes.
- When adding new enum values:
  - If the enum is stored by name, no migration mapping is needed unless renaming existing values.
  - If a curated code converter exists (e.g., `LanguageCode`), update the converter and add data migration if needed.

## EF Core configuration

- Use `modelBuilder.ApplyConfigurationsFromAssembly(...)` to apply entity configurations.
- Keep `OnModelCreating` minimal: apply configurations, then conventions.
- Prefer `ConfigureConventions` for global rules (enum storage, LanguageCode converter).
- Use `.AsNoTracking()` for query/read-side EF queries unless tracking is required.

## Ports and implementations

- Place ports in `Application`:
  - Read ports: `IProjectQueries`, `IKeywordQueries`, etc.
  - External boundary ports: `IRecaptchaValidator` under `Application.Abstractions.Security`.
- Implement ports in `Infrastructure` (EF query services) or `Web.Api` (request context adapters).
- Do not leak EF Core entities to `Application` models. Project to `*Model` in query implementations.

## Web.Api conventions (Carter/minimal APIs)

- Endpoints should be thin:
  - No domain logic.
  - No culture mapping logic inside endpoints.
  - No direct EF usage in endpoints.
- Use typed results and centralized result mapping:
  - `Kulku.Web.Api.Http.ResultHttpExtensions`
  - `Kulku.Web.Api.Http.ProblemDetailsFactory`
- Prefer returning `ProblemDetails` consistently for failures.

## Result/ProblemDetails mapping

- Use centralized mapping:
  - Map `ErrorCodes.NotFound` -> `404`.
  - Map validation errors -> `400` with `errors` extension grouped by code.
  - Default failures -> `400` (or expand mapping to 401/403/409/500 as codes exist).
- Avoid duplicating HTTP mapping in endpoints.

## Code style and quality

- Favor explicitness and clarity over cleverness.
- Keep methods small; extract private helpers where it improves readability.
- No “magic strings” for persisted codes—use constants or converters.
- Use XML doc comments on public APIs and cross-cutting abstractions.
- Validate input at the edges (API/UI). Do not scatter ad-hoc validation deep in infrastructure.

## When generating new code, Copilot must

- Propose changes consistent with these conventions.
- Avoid introducing new dependencies that break layer boundaries.
- Prefer minimal, coherent structures over excessive “purist” sharding.
- Suggest migrations when changing persisted enum representations or data.
