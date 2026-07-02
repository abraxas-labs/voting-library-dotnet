// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

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

    /// <inheritdoc cref="IUserService.GetReadySecondFactorProviders"/>
    /// <summary>
    /// Returns all second factor providers.
    /// </summary>
    public Task<IReadOnlySet<V1SecondFactorProvider>> GetReadySecondFactorProviders(string loginId)
        => Task.FromResult<IReadOnlySet<V1SecondFactorProvider>>(new HashSet<V1SecondFactorProvider> { V1SecondFactorProvider.NEVIS });

    /// <inheritdoc cref="IUserService.RequestSecondFactor"/>
    /// <summary>
    /// Returns a string of a random generated guid.
    /// </summary>
    public Task<SecondFactor> RequestSecondFactor(string loginId, V1SecondFactorProvider provider, string? message = null)
        => Task.FromResult(new SecondFactor(V1SecondFactorProvider.OTP));

    /// <inheritdoc cref="IUserService.VerifySecondFactor"/>
    /// <summary>
    /// Returns true if the code or token jwts matches the <see cref="SecureConnectTestDefaults.MockedVerified2faId"/>.
    /// </summary>
    public Task<bool> VerifySecondFactor(string loginId, V1SecondFactorProvider provider, string? code = null, ICollection<string>? nevisTokenJwtIds = null, CancellationToken ct = default)
    {
        var nevisOk = nevisTokenJwtIds?.Contains(SecureConnectTestDefaults.MockedVerified2faId) ?? false;
        var otpOk = code == SecureConnectTestDefaults.MockedVerified2faId;
        return Task.FromResult(nevisOk || otpOk);
    }
}
