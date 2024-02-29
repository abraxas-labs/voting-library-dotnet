// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.Iam.Exceptions;
using Voting.Lib.Iam.Models;

namespace Voting.Lib.Iam.Store;

/// <summary>
/// Interface for retrieving authentication values.
/// </summary>
public interface IAuth
{
    /// <summary>
    /// Gets a value indicating whether the authentication values have been set.
    /// </summary>
    public bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the user.
    /// </summary>
    /// <exception cref="NotAuthenticatedException">In case <see cref="IsAuthenticated"/> is false.</exception>
    public User User { get; }

    /// <summary>
    /// Gets the tenant.
    /// </summary>
    /// <exception cref="NotAuthenticatedException">In case <see cref="IsAuthenticated"/> is false.</exception>
    public Tenant Tenant { get; }

    /// <summary>
    /// Gets the roles of the current user.
    /// </summary>
    /// <exception cref="NotAuthenticatedException">In case <see cref="IsAuthenticated"/> is false.</exception>
    public IReadOnlyCollection<string> Roles { get; }

    /// <summary>
    /// Gets the permissions of the current user.
    /// </summary>
    /// <exception cref="NotAuthenticatedException">In case <see cref="IsAuthenticated"/> is false.</exception>
    public IReadOnlyCollection<string> Permissions { get; }

    /// <summary>
    /// Gets the access token of the current user.
    /// <exception cref="NotAuthenticatedException">In case <see cref="IsAuthenticated"/> is false.</exception>
    /// </summary>
    public string AccessToken { get; }
}
