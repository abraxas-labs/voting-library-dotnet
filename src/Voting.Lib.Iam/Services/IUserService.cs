// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Services.ApiClient.Identity;

namespace Voting.Lib.Iam.Services;

/// <summary>
/// Interface for the user service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Fetches a user by its login ID or returns null if none is found.
    /// </summary>
    /// <param name="loginId">The login id.</param>
    /// <param name="includeDeleted">Whether to include deleted users.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<User?> GetUser(string loginId, bool includeDeleted);

    /// <summary>
    /// Returns a set of second factor providers that are ready to be used for the given login ID.
    /// </summary>
    /// <param name="loginId">The login ID.</param>
    /// <returns>The set of ready second factor providers.</returns>
    Task<IReadOnlySet<V1SecondFactorProvider>> GetReadySecondFactorProviders(string loginId);

    /// <summary>
    /// Requests a new 2fa for a login ID.
    /// </summary>
    /// <param name="loginId">The login id.</param>
    /// <param name="provider">The provider of the 2fa.</param>
    /// <param name="message">The optional message.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, resolving to the 2fa.</returns>
    Task<SecondFactor> RequestSecondFactor(string loginId, V1SecondFactorProvider provider, string? message = null);

    /// <summary>
    /// Verifies a 2fa for a login id and returns true if the verification succeeded or false if not.
    /// </summary>
    /// <param name="loginId">The login id.</param>
    /// <param name="provider">A <see cref="V1SecondFactorProvider"/> for the 2fa.</param>
    /// <param name="code">The verification code (if OTP, voice, …).</param>
    /// <param name="nevisTokenJwtIds">The jwt ids of the 2fa (if NEVIS).</param>
    /// <param name="ct">The cancellation token.</param>
    /// <exception cref="Voting.Lib.Iam.Exceptions.VerifySecondFactorTimeoutException">Throws if the request runs into a timeout.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, resolving to the bool result.</returns>
    Task<bool> VerifySecondFactor(
        string loginId,
        V1SecondFactorProvider provider,
        string? code = null,
        ICollection<string>? nevisTokenJwtIds = null,
        CancellationToken ct = default);
}
