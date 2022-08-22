// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Voting.Lib.Iam.AuthenticationScheme;

/// <summary>
/// Handles role token related things.
/// </summary>
public interface IRoleTokenHandler
{
    /// <summary>
    /// Returns a list of roles for the provided subject/tenantId.
    /// </summary>
    /// <param name="subjectToken">The token of the user.</param>
    /// <param name="tenantId">The id of the tenant for which the roles should be loaded.</param>
    /// <param name="apps">
    /// The shortcuts of the apps for which the roles should be loaded.
    /// Provided apps must be whitelisted via the options.
    /// This has an affect only if <see cref="SecureConnectOptions.LimitRolesToAppHeaderApps"/> is true.
    /// </param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<IReadOnlyCollection<string>> GetRoles(string subjectToken, string tenantId, IEnumerable<string>? apps = null);
}
