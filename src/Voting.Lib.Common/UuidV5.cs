// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Security.Cryptography;
using System.Text;
using Voting.Lib.Common.Extensions;

namespace Voting.Lib.Common;

/// <summary>
/// Utils to create UUID v5 (see rfc 4122).
/// </summary>
public static class UuidV5
{
    private const int Version = 5;

    /// <summary>
    /// Generates an UUID v5 according to RFC 4122.
    /// Copied and modified from https://github.com/StephenCleary/Guids.
    /// </summary>
    /// <param name="namespace">The namespace.</param>
    /// <param name="name">The name.</param>
    /// <returns>The generated UUID.</returns>
    public static Guid Create(Guid @namespace, string name)
        => Create(@namespace, Encoding.ASCII.GetBytes(name));

    /// <summary>
    /// Generates an UUID v5 according to RFC 4122.
    /// Copied and modified from https://github.com/StephenCleary/Guids.
    /// </summary>
    /// <param name="namespace">The namespace.</param>
    /// <param name="name">The name.</param>
    /// <returns>The generated UUID.</returns>
    public static Guid Create(Guid @namespace, byte[] name)
    {
        var namespaceBytes = @namespace.ToByteArray();
        EndianSwap(namespaceBytes);

        // SHA1 required by the rfc.
#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
#pragma warning disable CA5350 // Do Not Use Weak Cryptographic Algorithms
        using var hasher = SHA1.Create();
#pragma warning restore CA5350 // Do Not Use Weak Cryptographic Algorithms
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms

        hasher.TransformBlock(namespaceBytes, 0, namespaceBytes.Length, null, 0);
        hasher.TransformFinalBlock(name, 0, name.Length);

        var guidBytes = new byte[GuidExtensions.GuidByteLength];
        Array.Copy(
            hasher.Hash ?? throw new InvalidOperationException(nameof(hasher.Hash) + " is null, this should never happen for a sha1 hash after the final block"),
            0,
            guidBytes,
            0,
            16);
        EndianSwap(guidBytes);

        // Variant RFC4122
        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80); // big-endian octet 8

        // Version
        guidBytes[7] = (byte)((guidBytes[7] & 0x0F) | (Version << 4)); // big-endian octet 6

        return new Guid(guidBytes);
    }

    /// <summary>
    /// Converts a big-endian GUID (dotnet implementation) to a little-endian UUID, or vice versa.
    /// </summary>
    /// <param name="guidBytes">The guid bytes.</param>
    private static void EndianSwap(byte[] guidBytes)
    {
        Swap(guidBytes, 0, 3);
        Swap(guidBytes, 1, 2);
        Swap(guidBytes, 4, 5);
        Swap(guidBytes, 6, 7);
    }

    private static void Swap(byte[] array, int index1, int index2)
        => (array[index1], array[index2]) = (array[index2], array[index1]);
}
