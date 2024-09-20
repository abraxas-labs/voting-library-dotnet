// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// Custom data passed along in a callback.
/// </summary>
public class CallbackCustomData
{
    /// <summary>
    /// Gets or sets the ID of the print job. Available only in a finished_editing callback.
    /// </summary>
    public int? PrintJobId { get; set; }
}
