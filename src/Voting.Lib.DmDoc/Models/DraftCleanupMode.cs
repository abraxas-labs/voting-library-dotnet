// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc.Models;

/// <summary>
/// The DmDoc draft cleanup mode.
/// </summary>
public enum DraftCleanupMode
{
    /// <summary>
    /// The draft should be deleted softly, which means flagging it for deletion in DmDoc and let the DmDoc clean it up according to its cleanup configuration.
    /// The print job id and all document contents are removed as well.
    /// </summary>
    Soft = 0,

    /// <summary>
    /// The draft should be deleted hard, which means it gets deleted immediately.
    /// The print job id and all document contents are removed as well.
    /// </summary>
    Hard = 1,

    /// <summary>
    /// The draft's content should only be removed, which means the draft metadata used during generation is getting removed immediately.
    /// The print job id and all document contents are still available.
    /// </summary>
    Content = 2,
}
