// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;

namespace Voting.Lib.Common;

/// <summary>
/// Helper class for parsing string to GUIDs.
/// </summary>
public static class GuidParser
{
    /// <summary>
    /// Parses a string to a GUID or throws an exception if the string is not a valid GUID.
    /// </summary>
    /// <param name="id">The string to parse.</param>
    /// <returns>The parsed GUID.</returns>
    /// <exception cref="ValidationException">Thrown when the string is not a valid GUID.</exception>
    public static Guid Parse(string id)
    {
        if (!Guid.TryParse(id, out var guid))
        {
            throw new ValidationException($"Could not parse GUID {id}");
        }

        return guid;
    }

    /// <summary>
    /// Parses a string to a GUID or returns null if the string is null.
    /// </summary>
    /// <param name="id">The string to parse.</param>
    /// <returns>The parsed GUID or null if the string is null.</returns>
    public static Guid? ParseNullable(string? id)
        => string.IsNullOrEmpty(id) ? null : Parse(id);
}
