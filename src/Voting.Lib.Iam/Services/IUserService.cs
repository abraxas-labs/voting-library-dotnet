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
    /// Requests a new 2fa for a login ID.
    /// </summary>
    /// <param name="loginId">The login id.</param>
    /// <param name="provider">The provider of the 2fa.</param>
    /// <param name="message">The message.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, resolving to the 2fa.</returns>
    Task<SecondFactor> RequestSecondFactor(string loginId, string provider, string message);

    /// <summary>
    /// Verifies a 2fa for a login id and returns true if the verification succeeded or false if not.
    /// </summary>
    /// <param name="loginId">The login id.</param>
    /// <param name="provider">A <see cref="V1SecondFactorProvider"/> for the 2fa.</param>
    /// <param name="tokenJwtIds">The jwt ids of the 2fa.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <exception cref="Voting.Lib.Iam.Exceptions.VerifySecondFactorTimeoutException">Throws if the request runs into a timeout.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, resolving to the bool result.</returns>
    Task<bool> VerifySecondFactor(string loginId, V1SecondFactorProvider provider, ICollection<string> tokenJwtIds, CancellationToken ct);
}
