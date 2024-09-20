// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Cli;

/// <summary>
/// Exit codes used by Voting.Lib.Cli.
/// </summary>
public static class ExitCodes
{
    /// <summary>
    /// All good :).
    /// </summary>
    public const int Ok = 0;

    /// <summary>
    /// Could not parse the arguments...
    /// </summary>
    public const int ParserError = -1;
}
