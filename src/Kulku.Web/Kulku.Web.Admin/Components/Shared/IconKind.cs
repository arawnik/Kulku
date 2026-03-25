namespace Kulku.Web.Admin.Components.Shared;

/// <summary>
/// Available icons for the <see cref="Icon"/> component.
/// Based on Bootstrap Icons SVG paths.
/// </summary>
public enum IconKind
{
    // ── Actions ──

    /// <summary>Plus sign — used for "Add" / "Create" actions.</summary>
    Plus,

    /// <summary>Pencil — used for "Edit" actions.</summary>
    Pencil,

    /// <summary>Trash can — used for "Delete" actions.</summary>
    Trash,

    /// <summary>Checkmark — used for "Confirm" actions.</summary>
    Check,

    // ── Navigation ──

    /// <summary>House (filled) — Home page.</summary>
    House,

    /// <summary>Lightbulb — Developer / ideas section.</summary>
    Lightbulb,

    /// <summary>Plus square (filled) — CV / content section.</summary>
    PlusSquare,

    /// <summary>File earmark person — CV / résumé.</summary>
    FileEarmarkPerson,

    /// <summary>Nested list — list / catalogue section.</summary>
    ListNested,

    /// <summary>Lock — authentication-required section.</summary>
    Lock,

    /// <summary>Person outline — register / user.</summary>
    Person,

    /// <summary>Person badge — login / credentials.</summary>
    PersonBadge,

    /// <summary>Person filled — authenticated user.</summary>
    PersonFill,

    /// <summary>Left arrow bar — collapse / toggle sidebar.</summary>
    ArrowBarLeft,

    /// <summary>Right arrow bar — expand sidebar.</summary>
    ArrowBarRight,

    /// <summary>File text — introduction / document.</summary>
    FileText,

    /// <summary>Mortarboard — education.</summary>
    Mortarboard,

    /// <summary>Briefcase — work experience.</summary>
    Briefcase,

    /// <summary>Folder — projects.</summary>
    Folder,

    /// <summary>Tags — keywords / categories.</summary>
    Tags,

    /// <summary>Journal/notebook — idea bank.</summary>
    Journal,

    // ── Indicators ──

    /// <summary>Chevron pointing down — expand / open.</summary>
    ChevronDown,

    /// <summary>Chevron pointing right — collapsed / closed.</summary>
    ChevronRight,

    /// <summary>Eye — visible / displayed.</summary>
    Eye,

    /// <summary>Eye with slash — hidden / not displayed.</summary>
    EyeSlash,
}
