// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// DmDoc callback actions.
/// </summary>
public enum CallbackAction
{
    /// <summary>
    /// Callback action for create (ex. creation of a draft).
    /// </summary>
    Create,

    /// <summary>
    /// Callback action when the creation of an object fails.
    /// </summary>
    CreateError,

    /// <summary>
    /// Callback action when the draft was finished (PDF is ready).
    /// </summary>
    FinishEditing,

    /// <summary>
    /// Callback action when the PDF generation has failed.
    /// </summary>
    FinishEditingError,
}
