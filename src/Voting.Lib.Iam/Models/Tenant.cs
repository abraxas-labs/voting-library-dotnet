// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.Models;

/// <summary>
/// Represents a tenant.
/// </summary>
public class Tenant
{
    /// <summary>
    /// Gets or sets the ID (system ID, automatically generated).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tenant ID (user supplied).
    /// </summary>
    public string TenantId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tenant name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
