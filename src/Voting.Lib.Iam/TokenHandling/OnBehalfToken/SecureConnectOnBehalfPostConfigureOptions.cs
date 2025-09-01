// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Microsoft.Extensions.Options;

namespace Voting.Lib.Iam.TokenHandling.OnBehalfToken;

/// <summary>
/// Validator for <see cref="SecureConnectOnBehalfOptions"/>.
/// </summary>
public class SecureConnectOnBehalfPostConfigureOptions : IPostConfigureOptions<SecureConnectOnBehalfOptions>
{
    /// <inheritdoc />
    public void PostConfigure(string? name, SecureConnectOnBehalfOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Resource))
        {
            throw new InvalidOperationException($"OnBehalf options must have a {nameof(options.Resource)} set.");
        }
    }
}
