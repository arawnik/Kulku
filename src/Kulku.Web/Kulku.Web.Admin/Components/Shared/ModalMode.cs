namespace Kulku.Web.Admin.Components.Shared;

/// <summary>
/// Controls whether an edit modal is used for creating a new entity or editing an existing one.
/// </summary>
public enum ModalMode
{
    /// <summary>
    /// The modal is creating a new entity.
    /// </summary>
    Create,

    /// <summary>
    /// The modal is editing an existing entity.
    /// </summary>
    Edit,
}
