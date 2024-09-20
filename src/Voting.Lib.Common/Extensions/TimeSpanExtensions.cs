// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace System;

/// <summary>
/// Extensions for <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>
    /// Converts the time span to a human readable string.
    /// </summary>
    /// <param name="ts">The time span to convert.</param>
    /// <returns>Returns a human readable string.</returns>
    public static string ToHumanReadableString(this TimeSpan ts)
    {
        return ts.TotalSeconds switch
        {
            < 60 => $"{ts:%s}s", // up to 59s
            < 60 * 60 => $"{ts:%m}m", // up to 59min
            < 60 * 60 * 24 => $"{ts:%h}h", // up to 1d
            _ => $"{ts:%d}d",
        };
    }
}
