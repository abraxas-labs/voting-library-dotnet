// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace System;

/// <summary>
/// Extensions for <see cref="decimal"/>.
/// </summary>
public static class DecimalExtensions
{
    /// <summary>
    /// Returns whether two decimals are approximately equal.
    /// </summary>
    /// <param name="x">The first decimal number.</param>
    /// <param name="y">The second decimal number.</param>
    /// <param name="precision">Floating point precision.</param>
    /// <returns>Approximate equality of the input numbers.</returns>
    public static bool ApproxEquals(this decimal x, decimal y, int precision = 10)
    {
        if (precision <= 0)
        {
            throw new ArgumentException("Precision must be a positive number");
        }

        var err = Math.Abs(x - y);
        var maxErr = (decimal)Math.Pow(10, -precision);
        return err < maxErr;
    }
}
