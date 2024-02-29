// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Voting.Lib.Iam.Models;

namespace Voting.Lib.Iam.Services;

/// <summary>
/// Interface for the tenant service.
/// </summary>
public interface ITenantService
{
    /// <summary>
    /// Fetches a tenant by its tenant ID or returns null if none is found.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="includeDeleted">Whether to include deleted tenants.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<Tenant?> GetTenant(string tenantId, bool includeDeleted);
}
