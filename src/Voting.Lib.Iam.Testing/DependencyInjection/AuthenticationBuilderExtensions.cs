// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.AspNetCore.Authentication;
using Voting.Lib.Iam.AuthenticationScheme;
using Voting.Lib.Iam.Testing.AuthenticationScheme;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for adding mocks to <see cref="AuthenticationBuilder"/>.
/// </summary>
public static class AuthenticationBuilderExtensions
{
    /// <summary>
    /// replaces the secure connect handler with a mock.
    /// </summary>
    /// <param name="builder">The AuthenticationBuilder.</param>
    /// <returns>The authentication builder instance.</returns>
    public static AuthenticationBuilder AddMockedSecureConnectScheme(this AuthenticationBuilder builder)
        => builder.AddScheme<SecureConnectOptions, AuthenticationHandlerMock>(
            SecureConnectDefaults.AuthenticationScheme,
            o => o.FetchRoleToken = false);
}
