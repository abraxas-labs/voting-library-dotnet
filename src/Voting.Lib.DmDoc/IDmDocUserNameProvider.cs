// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc;

/// <summary>
/// User name provider for DmDoc.
/// </summary>
public interface IDmDocUserNameProvider
{
    /// <summary>
    /// Gets the user name.
    /// </summary>
    string UserName { get; }
}
