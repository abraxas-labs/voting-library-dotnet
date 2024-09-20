// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Ech;

/// <summary>
/// Provides eCH message IDs.
/// </summary>
public interface IEchMessageIdProvider
{
    /// <summary>
    /// Creates a new unique message ID.
    /// </summary>
    /// <returns>The created message ID.</returns>
    string NewId();
}
