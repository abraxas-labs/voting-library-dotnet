// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DocPipe;

/// <summary>
/// Interface for a DocPipe URL builder.
/// </summary>
public interface IDocPipeUrlBuilder
{
    /// <summary>
    /// Gets the jobs URL.
    /// </summary>
    /// <returns>The jobs URL.</returns>
    string Jobs();
}
