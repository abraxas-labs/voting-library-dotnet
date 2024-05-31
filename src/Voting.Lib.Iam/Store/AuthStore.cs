// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Voting.Lib.Iam.Exceptions;
using Voting.Lib.Iam.Models;

namespace Voting.Lib.Iam.Store;

/// <summary>
/// Default implementation of <see cref="IAuth"/> and <see cref="IAuthStore"/>.
/// </summary>
internal sealed class AuthStore : IAuth, IAuthStore
{
    private readonly ILogger<AuthStore> _logger;
    private User _user = new();
    private Tenant _tenant = new();
    private string _accessToken = string.Empty;
    private IReadOnlyCollection<string> _roles = Array.Empty<string>();
    private IReadOnlyCollection<string> _permissions = Array.Empty<string>();

    public AuthStore(ILogger<AuthStore> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public bool IsAuthenticated { get; private set; }

    /// <inheritdoc />
    public User User => GetAuthValue(_user);

    /// <inheritdoc />
    public Tenant Tenant => GetAuthValue(_tenant);

    /// <inheritdoc />
    public string AccessToken => GetAuthValue(_accessToken);

    /// <inheritdoc />
    public IReadOnlyCollection<string> Roles => GetAuthValue(_roles);

    /// <inheritdoc />
    public IReadOnlyCollection<string> Permissions => GetAuthValue(_permissions);

    /// <inheritdoc />
    public void SetValues(string accessToken, User user, Tenant tenant, IEnumerable<string>? roles, IEnumerable<string>? permissions = null)
    {
        if (IsAuthenticated)
        {
            throw new AlreadyAuthenticatedException(
                $"Request is currently authenticated as {_user} on tenant {_tenant}, tried to switch to {user.Loginid} on {tenant.Id}.");
        }

        IsAuthenticated = true;
        _accessToken = accessToken;
        _user = user;
        _tenant = tenant;
        _roles = roles?.ToArray() ?? Array.Empty<string>();
        _permissions = permissions?.ToArray() ?? Array.Empty<string>();
    }

    // Note: The log scope needs to be started manually at the right place, since log scopes don't flow up in the call stack, only down.
    // Method calls receive the same log scope as the parent. If child methods change the log scope, that won't be visible in the parent.
    // If we define a log scope in the AuthenticationSchemeHandler, it won't affect the executed code in the gRPC/REST methods,
    // since the "async context" (where the log scope information is stored) has been cleared.
    public IDisposable? StartLogScope()
    {
        return _logger.BeginScope(new Dictionary<string, object>
        {
            ["TenantId"] = _tenant.Id,
            ["Roles"] = _roles,
        });
    }

    private T GetAuthValue<T>(T value)
    {
        if (!IsAuthenticated)
        {
            throw new NotAuthenticatedException();
        }

        return value;
    }
}
