// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;
using Voting.Lib.Iam.Exceptions;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Services.ApiClient.Identity;

namespace Voting.Lib.Iam.Services;

/// <summary>
/// The implementation of the user service client.
/// </summary>
public class UserService : IUserService
{
    private readonly ISecureConnectIdentityServiceClient _client;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="client">The secure connect identity service client.</param>
    /// <param name="logger">The logger.</param>
    public UserService(ISecureConnectIdentityServiceClient client, ILogger<UserService> logger)
    {
        _client = client;
        _logger = logger;
    }

    /// <inheritdoc cref="IUserService.GetUser"/>
    public async Task<User?> GetUser(string loginId, bool includeDeleted)
    {
        try
        {
            var user = await _client.IdentityService_GetUserByLoginIdAsync(loginId, includeDeleted).ConfigureAwait(false);
            return new User
            {
                Username = user.Username,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Loginid = user.Loginid,
                Servicename = user.Servicename,
            };
        }
        catch (ApiException e) when (e.StatusCode == StatusCodes.Status404NotFound)
        {
            return null;
        }
    }

    /// <inheritdoc cref="IUserService.RequestSecondFactor"/>
    public async Task<SecondFactor> RequestSecondFactor(string loginId, string provider, string message)
    {
        var request = new V1SecondFactorProviderItem { Provider = provider, Message = message };
        var response = await _client.IdentityService_RequestSecondFactorByLoginIdAsync(loginId, request).ConfigureAwait(false);
        return new SecondFactor(response.Code, response.Nevis.Qr, response.Nevis.Nevis_action_token_jtis);
    }

    /// <inheritdoc cref="IUserService.VerifySecondFactor"/>
    public async Task<bool> VerifySecondFactor(string loginId, V1SecondFactorProvider provider, string secondFactorAuthId, ICollection<string> tokenJwtIds, CancellationToken ct)
    {
        var request = new V1VerifySecondFactorRequest { Provider = provider, Code = secondFactorAuthId, Nevis = new V1NevisVerification { Nevis_action_token_jtis = tokenJwtIds } };
        try
        {
            await _client.IdentityService_VerifySecondFactorByLoginIdAsync(loginId, request, ct).ConfigureAwait(false);
            return true;
        }
        catch (ApiException e) when (e.StatusCode == StatusCodes.Status504GatewayTimeout)
        {
            throw new VerifySecondFactorTimeoutException();
        }
        catch (ApiException e)
        {
            _logger.LogError(SecurityLogging.SecurityEventId, e, "During verification of the 2fa a server side error occurred.");
            return false;
        }
    }
}
