﻿using Kulku.Contract.Enums;
using Kulku.Domain.Constants;

namespace Kulku.Domain.Projects;

/// <summary>
/// Represents a localized version of a project entity, containing language-specific data.
/// </summary>
public class ProjectTranslation
{
    /// <summary>
    /// Unique identifier for the project translation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Foreign key reference to the parent project.
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// The language code that identifies the language of this translation.
    /// </summary>
    public LanguageCode Language { get; set; } = Defaults.Language;

    /// <summary>
    /// The localized name of the project.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A short, localized summary or tagline for the project.
    /// </summary>
    public string Info { get; set; } = string.Empty;

    /// <summary>
    /// A detailed, localized description of the project.
    /// Can be left empty.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Navigation property for the related <see cref="Project"/> entity.
    /// </summary>
    public Project Project { get; set; } = null!;
}
