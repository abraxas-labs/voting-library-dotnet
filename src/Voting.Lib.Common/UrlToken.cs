// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace Voting.Lib.Common;

/// <summary>
/// Represents a URL-safe secret token, e.g., for email confirmation or other actions.
/// Tokens are prefixed with <c>ut.1-</c> and the rest of the value is Base64 URL encoded.
/// </summary>
public readonly record struct UrlToken
{
    /// <summary>
    /// A prefix that identifies the token's type and version.
    /// This enables secret scanning tools to be configured to detect these tokens
    /// and allows revocation of all tokens of a specific type or version in the future.
    /// </summary>
    public const string Prefix = "ut.1-";

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlToken"/> struct
    /// from the specified string value.
    /// </summary>
    /// <param name="value">The token string, which must start with <c>ut.1-</c>
    /// and have a valid Base64 URL-encoded value after the prefix.</param>
    /// <exception cref="ArgumentException">Thrown if the value does not start with the prefix
    /// or if the rest of the string is not valid Base64 URL encoding.</exception>
    public UrlToken(string value)
    {
        if (!value.StartsWith(Prefix))
        {
            throw new ValidationException("Invalid token format.");
        }

        var valueSpan = value.AsSpan();
        if (!Base64Url.IsBase64Url(valueSpan[Prefix.Length..]))
        {
            throw new ValidationException("Invalid token format.");
        }

        Value = value;
    }

    /// <summary>
    /// Gets the underlying string value of the token.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Converts the <see cref="UrlToken"/> to its underlying string value.
    /// </summary>
    /// <param name="token">The token to convert.</param>
    public static implicit operator string(UrlToken token) => token.Value;

    /// <summary>
    /// Converts a string value to a <see cref="UrlToken"/>.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    public static implicit operator UrlToken(string value) => new(value);

    /// <summary>
    /// Creates a new cryptographically secure <see cref="UrlToken"/> of the specified size in bytes.
    /// The token is encoded using Base64 URL encoding and prefixed with <c>ut.1-</c>.
    /// </summary>
    /// <param name="size">The size in bytes of the generated token. Defaults to 64 bytes.</param>
    /// <returns>A new <see cref="UrlToken"/> instance.</returns>
    public static UrlToken New(int size = 64)
    {
        Span<byte> data = stackalloc byte[size];
        RandomNumberGenerator.Fill(data);
        return new UrlToken(Prefix + Base64Url.Encode(data));
    }

    /// <summary>
    /// Returns the string representation of the <see cref="UrlToken"/>.
    /// </summary>
    /// <returns>The underlying string value of the token.</returns>
    public override string ToString() => Value;
}
