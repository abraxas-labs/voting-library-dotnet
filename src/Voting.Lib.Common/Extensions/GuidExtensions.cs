// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;

namespace Voting.Lib.Common.Extensions;

/// <summary>
/// Extensions for <see cref="Guid"/>.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// The length of a guid in bytes.
    /// Required length of span passed to <see cref="WriteBytesAsRfc4122"/>.
    /// </summary>
    public const int GuidByteLength = 16;

    /// <summary>
    /// Writes the bytes representation of a <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The guid.</param>
    /// <param name="destination">The destination span.</param>
    /// <exception cref="InvalidOperationException">If the guid could not be written correctly.</exception>
    public static void WriteBytesAsRfc4122(this Guid guid, Span<byte> destination)
    {
        if (!guid.TryWriteBytes(destination))
        {
            throw new InvalidOperationException("Could not write guid");
        }

        if (!BitConverter.IsLittleEndian)
        {
            return;
        }

        // dotnet serializes first 3 fields of the guid according to the endianness of the current architecture.
        // RFC4122 requires big endian.
        // If this code runs on a little endian architecture, the first 3 fields (4 byte, 2 byte, 2 byte) are reversed to match RFC4122.
        (destination[0], destination[1], destination[2], destination[3]) = (destination[3], destination[2], destination[1], destination[0]);
        (destination[4], destination[5]) = (destination[5], destination[4]);
        (destination[6], destination[7]) = (destination[7], destination[6]);
    }
}
