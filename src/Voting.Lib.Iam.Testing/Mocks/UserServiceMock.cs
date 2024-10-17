// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Services;
using Voting.Lib.Iam.Services.ApiClient.Identity;
using Voting.Lib.Iam.Testing.AuthenticationScheme;

namespace Voting.Lib.Iam.Testing.Mocks;

/// <summary>
/// A user service mock.
/// </summary>
public class UserServiceMock : IUserService
{
    /// <inheritdoc cref="IUserService.GetUser"/>
    /// <summary>
    /// Searches the mocked user in the list of all mocked users by the provided login id.
    /// Returns null if none is found.
    /// </summary>
    public Task<User?> GetUser(string loginId, bool includeDeleted)
        => Task.FromResult(SecureConnectTestDefaults.MockedUsers.Find(t => t.Loginid == loginId));

    /// <inheritdoc cref="IUserService.RequestSecondFactor"/>
    /// <summary>
    /// Returns a string of a random generated guid.
    /// </summary>
    public Task<SecondFactor> RequestSecondFactor(string loginId, string provider, string message)
        => Task.FromResult(new SecondFactor(Guid.NewGuid().ToString(), new List<string>()));

    /// <inheritdoc cref="IUserService.VerifySecondFactor"/>
    /// <summary>
    /// Returns true if the code matches the <see cref="SecureConnectTestDefaults.MockedVerified2faId"/>.
    /// </summary>
    public Task<bool> VerifySecondFactor(string loginId, V1SecondFactorProvider provider, ICollection<string> tokenJwtIds, CancellationToken ct)
        => Task.FromResult(tokenJwtIds.Contains(SecureConnectTestDefaults.MockedVerified2faId));
}
