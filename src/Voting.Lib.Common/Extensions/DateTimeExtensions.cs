// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace System;

/// <summary>
/// Extensions for <see cref="DateTime"/>.
/// </summary>
public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo _chTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/Zurich");

    /// <summary>
    /// Returns a new Utc <see cref="DateTime"/> that takes place the next day at 00:00.
    /// If <paramref name="chTimeZone"/> is set to true, it will return a slightly lower utc time (22:00 or 23:00 depending on european summer time).
    /// On alpine runtimes, the package tzdata is required.
    /// </summary>
    /// <param name="dateTime">DateTime.</param>
    /// <param name="chTimeZone">Whether the next date is in swiss time or not.</param>
    /// <returns>A Utc <see cref="DateTime"/>.</returns>
    public static DateTime NextUtcDate(this DateTime dateTime, bool chTimeZone = false)
    {
        if (dateTime.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException($"DateTime must be of kind {DateTimeKind.Utc}");
        }

        if (!chTimeZone)
        {
            return dateTime.AddDays(1).Date;
        }

        var chDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, _chTimeZoneInfo);
        var chNextDate = chDateTime.AddDays(1).Date;
        return TimeZoneInfo.ConvertTimeToUtc(chNextDate, _chTimeZoneInfo);
    }
}
