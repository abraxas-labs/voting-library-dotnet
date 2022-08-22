// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Voting.Lib.Common;

/// <summary>
/// A helper class to calculate hashes.
/// </summary>
public static class HashUtil
{
    /// <summary>
    /// Calculates the SHA256 hash of the input.
    /// </summary>
    /// <param name="input">The input to hash.</param>
    /// <returns>Returns the SHA256 hash of the input.</returns>
    public static string GetSHA256Hash(string input)
    {
        using var sha256HashAlgorithm = SHA256.Create();
        var data = sha256HashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        var sb = new StringBuilder();
        foreach (var t in data)
        {
            sb.Append(t.ToString("x2", CultureInfo.InvariantCulture));
        }

        return sb.ToString();
    }
}
