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
  - **Presentation layers (`Web.Api`, `Web.Admin`) must never inject query port interfaces directly.** Always route reads through `IQueryHandler<TQuery, TResult>`. Query ports are internal implementation details consumed only by `Application` handlers and implemented by `Infrastructure`.

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

## Read/write data access (CQRS ports)

The architecture uses two parallel port families for data access, one for each side of CQRS. Both follow the same enforcement rule: **presentation layers must never inject them directly** — always go through `IQueryHandler<>` or `ICommandHandler<>`.

- **Query ports (read-side)**
  - Defined in `Application/*/Ports/` (e.g. `IProjectQueries`, `IIdeaQueries`).
  - Consumed by query handlers (`GetProjects.Handler`, `GetIdeas.Handler`, etc.) inside `Application`.
  - Implemented in `Infrastructure/Queries/` using EF Core with `.AsNoTracking()` and `LeftJoin` for translations.
  - Return `*Model` records — never leak EF Core entities to `Application`.

- **Repositories (write-side)**
  - Defined in `Domain/Repositories/` (e.g. `IProjectRepository`, `IIdeaRepository`).
  - All inherit from `IEntityRepository<T>` which provides `GetByIdAsync`, `Add`, and `Remove`.
  - Consumed by command handlers (`CreateProject.Handler`, `UpdateIdea.Handler`, etc.) inside `Application`, always paired with `IUnitOfWork`.
  - Implemented in `Infrastructure/Repositories/` using EF Core tracked entities.

- **External boundary ports**
  - Non-data ports like `IRecaptchaValidator` live under `Application.Abstractions.Security`.
  - Implemented in `Infrastructure` (HTTP clients, etc.) or `Web.Api` (request context adapters like `ILanguageContext`).

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

## Blazor Admin CRUD pattern

Each admin section for a translatable entity follows a consistent vertical slice:

- **Page code-behind** (`*.razor.cs`):
  - Primary constructor DI for query and command handlers.
  - `ModalMode? _modalMode` tracks whether the modal is in Create or Edit mode (null = closed).
  - `HandleCreate()` — loads parent entities (e.g. companies, institutions) once, builds a blank model with translations for all `Defaults.SupportedCultures`, sets `_modalMode = ModalMode.Create`.
  - `HandleEdit(Guid)` — fetches detail via query handler, sets `_modalMode = ModalMode.Edit`.
  - `HandleSave(model)` — routes to create or update handler based on `_modalMode`. On success: close editor, reload list. On `IValidationResult`: push field errors via `_editModal?.SetServerErrors(...)`. On other failure: set `_errorMessage` banner.
  - `HandleDelete(Guid)` — calls delete handler, reloads list on success.
  - `CloseEditor()` — resets `_modalMode`, model, and `_errorMessage` to null.

- **Edit modal** (`*EditModal.razor`):
  - `Mode` parameter (`ModalMode`, defaults to `Edit`).
  - Parent entity dropdown (e.g. institution, company) shown only in Create mode.
  - `ServerValidation @ref` inside `EditForm` for server error display.
  - Exposes `SetServerErrors(IEnumerable<Error>)` for the parent page to call.
  - Adaptive title and submit button text based on `Mode`.
  - `OnParametersSet` rebuilds form only when the model ID changes (prevents re-render thrashing).
  - Inner form model classes with `[Required]` on mandatory fields (e.g. `Title`) for client-side validation.

- **Card component** (`*Card.razor`):
  - `OnEdit` and `OnDelete` `EventCallback<Guid>` parameters.
  - Edit button (`btn-outline-primary`) and Delete button (`btn-outline-danger`) with `<Icon>` components.
  - `ConfirmDialog` for delete confirmation — no inline confirm/cancel, use the shared modal.

- **Ordering for date-ranged entries**:
  - Ongoing entries (null `EndDate`) appear first.
  - Then sort by `EndDate` descending, then `StartDate` descending as tiebreaker.
  - Apply this sort in the page code-behind after receiving query results, not in the query itself.

## Server validation in Blazor forms

- Use the `ServerValidation` component (`Components/Shared/ServerValidation.cs`) inside `EditForm` alongside `DataAnnotationsValidator`.
- The parent page calls `SetServerErrors(validation.Errors)` via `@ref` after a failed save returns `IValidationResult`.
- `ServerValidation` uses `ToFieldIdentifier` to walk dotted property paths (e.g. `Translations[0].Title`) and resolve them to the correct `FieldIdentifier` on the form model.
- Property lookup is **case-insensitive** (`BindingFlags.IgnoreCase`) because server validators use `nameof(parameter)` which produces camelCase, while form model properties are PascalCase.
- Do **NOT** subscribe to `OnFieldChanged` — server errors are cleared only on the next `OnValidationRequested` (triggered by `EditContext.Validate()`). Subscribing to `OnFieldChanged` causes server errors to disappear during Blazor re-render cascades.
- Use `BootstrapFieldCssClassProvider` on every `EditContext` for Bootstrap `is-valid`/`is-invalid` CSS classes.

## Result and IValidationResult handling in Blazor

- Create commands return `Result<Guid>`, update/delete commands return `Result`.
- `Result<Guid>` **cannot** implicitly convert to `Result` — handle create and update paths separately.
- Always check `result is IValidationResult validation` **before** any conversion or error access, because converting with `Result.Failure(result.Error!)` loses the `IValidationResult` interface.
- For validation failures: push field-level errors via `SetServerErrors()`. Do **not** also set `_errorMessage` banner.
- For non-validation failures (not found, server error): set `_errorMessage` banner only.

## Shared component conventions (Web.Admin)

Reusable components live in `Components/Shared/` and are globally available via `Components/_Imports.razor`:
- **Never add per-file `@using Kulku.Web.Admin.Components.Shared`** — it is already imported globally.
- `Icon.razor` + `IconKind.cs` — Inline SVG icons using `fill="currentColor"` and `vertical-align: -.125em`. Add new icons to the `IconKind` enum and the `GetPath()` switch expression.
- `ConfirmDialog.razor` — Modal confirmation dialog with customizable title, message, button text/style/icon.
- `ModalDialog.razor` — Base modal wrapper with `ChildContent` and `FooterContent` render fragments.
- `ServerValidation.cs` — Server error → `EditContext` bridge (see section above).
- `BootstrapFieldCssClassProvider.cs` — Bootstrap-compatible field CSS class provider.
- `ModalMode.cs` — `Create`/`Edit` enum for dual-mode modals.

## DI registration discipline

- **No assembly scanning** — every handler and port implementation is explicitly registered.
- Command/query handlers: `ApplicationDependencyInjection.cs` (`AddApplication()`).
- Query implementations and repositories: `InfrastructureDependencyInjection.cs` (`AddInfrastructure()`).
- When adding a new use case, always update **both** files.
- Group registrations by feature (experience, education, projects, etc.) for readability.

## Command validation pattern

- Shared validation logic lives in a `*CommandValidator` static class (e.g. `ExperienceCommandValidator`) with a `Validate()` method.
- Both Create and Update handlers call the same validator to avoid duplication.
- Returns `Error[]` — empty array means valid.
- Use `Error.Validation(fieldPath, message)` where `fieldPath` matches the form model structure (e.g. `"translations[0].Title"`).
- `nameof()` produces camelCase field names — this is intentional. The `ServerValidation.ToFieldIdentifier` method resolves them case-insensitively via `BindingFlags.IgnoreCase`.
