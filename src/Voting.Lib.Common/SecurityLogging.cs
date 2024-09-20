// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.Logging;

namespace Voting.Lib.Common;

/// <summary>
/// A helper class for security related logs.
/// </summary>
public static class SecurityLogging
{
    /// <summary>
    /// Defines an <see cref="EventId"/> that should be used for all security related logs.
    /// </summary>
    public static readonly EventId SecurityEventId = new(1002, "Security event");
}
