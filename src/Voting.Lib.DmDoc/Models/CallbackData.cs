// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// The data DmDoc sends in a webhook callback.
/// </summary>
public class CallbackData
{
    /// <summary>
    /// Gets or sets the action of this callback.
    /// </summary>
    public CallbackAction Action { get; set; }

    /// <summary>
    /// Gets or sets the object ID of this callback (ex. the draft ID).
    /// </summary>
    public int ObjectId { get; set; }

    /// <summary>
    /// Gets or sets the object class of this callback (ex. Draft).
    /// </summary>
    public string ObjectClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the message of this callback (ex. 'Draft 123 was created' or 'Error while creating Draft: ...').
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the custom data of this callback, which varies depending on the callback action.
    /// </summary>
    public CallbackCustomData? Data { get; set; }
}
