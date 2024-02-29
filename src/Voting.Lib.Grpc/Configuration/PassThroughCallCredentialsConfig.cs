// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Grpc.Configuration;

/// <summary>
/// Configuration on how to pass through user information.
/// </summary>
/// <param name="ThrowIfUnauthenticated">Whether to throw if there is no user auth info stored.</param>
/// <param name="TokenHeaderName">The name of the token header.</param>
/// <param name="TokenHeaderValuePrefix">The prefix to be added before the token.</param>
/// <param name="TenantHeaderName">The name of the tenant header.</param>
public record PassThroughCallCredentialsConfig(
    bool ThrowIfUnauthenticated = false,
    string TokenHeaderName = "Authorization",
    string TokenHeaderValuePrefix = "Bearer ",
    string TenantHeaderName = "x-tenant");
