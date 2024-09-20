// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Common;

/// <summary>
/// Languages used in the VOTING context.
/// </summary>
public static class Languages
{
    /// <summary>
    /// The german language.
    /// </summary>
    public const string German = "de";

    /// <summary>
    /// The french language.
    /// </summary>
    public const string French = "fr";

    /// <summary>
    /// The italian language.
    /// </summary>
    public const string Italian = "it";

    /// <summary>
    /// The romansh language.
    /// </summary>
    public const string Romansh = "rm";

    /// <summary>
    /// Gets all languages used in the VOTING context.
    /// </summary>
    public static IReadOnlyList<string> All { get; } = new List<string> { German, French, Italian, Romansh };
}
