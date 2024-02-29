// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;

namespace Voting.Lib.Common;

/// <summary>
/// Utility class for calculating the age from a date of birth.
/// </summary>
public static class AgeUtil
{
    /// <summary>
    /// Calculates the age (of a person) on a certain reference date. Rough algorithm: age = <see paramref="referenceDate"/> - <see paramref="dateOfBirth"/>.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <param name="referenceDate">The reference date to be used in the calculation. Must be greater or equal to <see paramref="dateOfBirth"/>.</param>
    /// <returns>The calculated age.</returns>
    public static int CalculateAge(DateOnly dateOfBirth, DateOnly referenceDate)
    {
        if (dateOfBirth > referenceDate)
        {
            throw new ArgumentException("The date of birth must be earlier or equal to the referenceDate", nameof(referenceDate));
        }

        var age = referenceDate.Year - dateOfBirth.Year;

        // Check if the birthday is later this year
        // We need to compare the dates in the year the person was born to account for leap years
        // For example, a person born in 2000-02-29 is not yet 5 on the date 2005-02-28
        // Using "dateOfBirth.AddYears(age) > today" would be incorrect
        if (dateOfBirth > referenceDate.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
