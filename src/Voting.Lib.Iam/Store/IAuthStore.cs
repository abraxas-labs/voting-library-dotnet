﻿// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.Iam.Models;

namespace Voting.Lib.Iam.Store;

/// <summary>
/// Interface for storing authentication values.
/// </summary>
public interface IAuthStore
{
    /// <summary>
    /// Set the current authentication values. Only call this method once per request/scope.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <param name="user">The current user.</param>
    /// <param name="tenant">The current tenant.</param>
    /// <param name="roles">The roles of the current user.</param>
    /// <param name="permissions">The permissions of the current user.</param>
    /// <exception cref="Exceptions.AlreadyAuthenticatedException">In case this method has already been called.</exception>
    void SetValues(string accessToken, User user, Tenant tenant, IEnumerable<string>? roles, IEnumerable<string>? permissions = null);
}
