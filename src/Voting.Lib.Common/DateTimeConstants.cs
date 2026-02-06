// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Common;

/// <summary>
/// Provides constants related to date and time.
/// </summary>
public static class DateTimeConstants
{
    /// <summary>
    /// The IANA time zone identifier for Zurich, Switzerland.
    /// </summary>
    public const string EuropeZurichTimeZoneId = "Europe/Zurich";

    /// <summary>
    /// The <see cref="TimeZoneInfo"/> for Zurich, Switzerland.
    /// </summary>
    public static readonly TimeZoneInfo EuropeZurichTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(EuropeZurichTimeZoneId);
}
