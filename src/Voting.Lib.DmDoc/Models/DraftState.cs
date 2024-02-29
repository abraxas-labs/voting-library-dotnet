// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// The DmDoc draft state.
/// </summary>
public enum DraftState
{
    /// <summary>
    /// The draft has been created, but still needs data input.
    /// </summary>
    DataInput = 2,

    /// <summary>
    /// The draft has been generated and can be edited if needed.
    /// In this state, the preview can be fetched or the final PDF can be generated.
    /// </summary>
    Editing = 3,

    /// <summary>
    /// The draft has been printed.
    /// </summary>
    Printed = 6,

    /// <summary>
    /// The draft creation has failed.
    /// </summary>
    Error = 7,

    /// <summary>
    /// The draft is being generated and may be in the generation queue.
    /// </summary>
    InGeneration = 12,
}
