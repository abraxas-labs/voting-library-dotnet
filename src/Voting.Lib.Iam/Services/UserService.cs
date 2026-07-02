// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Voting.Lib.Common;
using Voting.Lib.Common.Extensions;
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
                PrimaryOrFirstEmail = user.Emails?.FirstOrDefault(e => e.Primary == true)?.Email
                                      ?? user.Emails?.FirstOrDefault()?.Email,
            };
        }
        catch (ApiException e) when (e.StatusCode == StatusCodes.Status404NotFound)
        {
            return null;
        }
    }

    /// <inheritdoc cref="IUserService.GetReadySecondFactorProviders"/>
    public async Task<IReadOnlySet<V1SecondFactorProvider>> GetReadySecondFactorProviders(string loginId)
    {
        var response = await _client.IdentityService_SecondFactorStatusAsync(loginId, Provider_status2.READY, false).ConfigureAwait(false);
        return response.Status.Where(x => x.Status == V1SecondFactorProviderStatus.READY)
            .Select(x => x.Provider)
            .WhereNotNull()
            .ToHashSet();
    }

    /// <inheritdoc cref="IUserService.RequestSecondFactor"/>
    public async Task<SecondFactor> RequestSecondFactor(string loginId, V1SecondFactorProvider provider, string? message = null)
    {
        var request = new V1SecondFactorProviderItem { Provider = provider.GetEnumMemberValue(), Message = message };
        var response = await _client.IdentityService_RequestSecondFactorByLoginIdAsync(loginId, request).ConfigureAwait(false);
        if (response.Nevis == null)
        {
            return new SecondFactor(provider);
        }

        return new SecondFactor(provider, new SecondFactorNevisInfo(response.Nevis.Qr, response.Nevis.Nevis_action_token_jtis));
    }

    /// <inheritdoc cref="IUserService.VerifySecondFactor"/>
    public async Task<bool> VerifySecondFactor(string loginId, V1SecondFactorProvider provider, string? code = null, ICollection<string>? nevisTokenJwtIds = null, CancellationToken ct = default)
    {
        var request = new V1VerifySecondFactorRequest { Provider = provider };

        if (!string.IsNullOrEmpty(code))
        {
            request.Code = code;
        }

        if (provider == V1SecondFactorProvider.NEVIS)
        {
            request.Nevis = new V1NevisVerification
            {
                Nevis_action_token_jtis = nevisTokenJwtIds?.ToList()
                                          ?? throw new InvalidOperationException(
                                              "Token JWT IDs must be provided for NEVIS verification."),
            };
        }

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
