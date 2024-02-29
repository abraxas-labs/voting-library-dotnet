// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.Services;
using Voting.Lib.Iam.Testing.AuthenticationScheme;

namespace Voting.Lib.Iam.Testing.Mocks;

/// <summary>
/// Mock implementation of the tenant service.
/// </summary>
public class TenantServiceMock : ITenantService
{
    /// <inheritdoc cref="ITenantService.GetTenant"/>
    /// <summary>
    /// Searches the mocked tenant in the list of all mocked tenants by the provided tenantId.
    /// Returns null if none is found.
    /// </summary>
    public Task<Tenant?> GetTenant(string tenantId, bool includeDeleted)
        => Task.FromResult(SecureConnectTestDefaults.MockedTenants.Find(t => t.Id == tenantId));
}
