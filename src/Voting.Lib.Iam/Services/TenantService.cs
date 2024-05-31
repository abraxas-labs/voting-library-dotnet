// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Services.ApiClient.Permission;

namespace Voting.Lib.Iam.Services;

internal class TenantService : ITenantService
{
    private readonly ISecureConnectPermissionServiceClient _client;

    public TenantService(ISecureConnectPermissionServiceClient client)
    {
        _client = client;
    }

    /// <inheritdoc cref="ITenantService.GetTenant" />
    public async Task<Tenant?> GetTenant(string tenantId, bool includeDeleted)
    {
        try
        {
            var tenant = await _client
                .PermissionService_GetTenantByIdAsync(tenantId, includeDeleted)
                .ConfigureAwait(false);
            return new Tenant
            {
                Id = tenant.Id,
                TenantId = tenant.TenantId,
                Name = tenant.Name,
            };
        }
        catch (ApiException e) when (e.StatusCode == StatusCodes.Status404NotFound)
        {
            return null;
        }
    }
}
