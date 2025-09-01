// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using Voting.Lib.Iam.Models;

namespace Voting.Lib.Iam.Testing.AuthenticationScheme;

/// <summary>
/// Default values for SecureConnect tests.
/// </summary>
public static class SecureConnectTestDefaults
{
    /// <summary>
    /// If any value is set, the user is authenticated, otherwise a 401 throws.
    /// </summary>
    public const string AuthHeader = "authorize";

    /// <summary>
    /// Each value of a header with a name x-roles is added as a role.
    /// </summary>
    public const string RolesHeader = "x-roles";

    /// <summary>
    /// Sets the tenant.
    /// </summary>
    public const string TenantHeader = "x-tenant";

    /// <summary>
    /// Userid of the mocked authentication handler.
    /// </summary>
    public const string UserHeader = "x-user";

    /// <summary>
    /// The default service token.
    /// </summary>
    public const string ServiceToken = "this-is-a-very-secret-token";

    /// <summary>
    /// The default on behalf token.
    /// </summary>
    public const string OnBehalfToken = "this-is-a-very-secret-on-behalf-token";

    /// <summary>
    /// The default mocked tenant.
    /// </summary>
    public static readonly Tenant MockedTenantDefault = new Tenant
    {
        Id = "000000000000000000",
        Name = "Default",
    };

    /// <summary>
    /// A default mocked tenant called uzwil.
    /// </summary>
    public static readonly Tenant MockedTenantUzwil = new Tenant
    {
        Id = "100100100100100100",
        Name = "Uzwil",
    };

    /// <summary>
    /// A default mocked tenant called st. gallen.
    /// </summary>
    public static readonly Tenant MockedTenantStGallen = new Tenant
    {
        Id = "200200200200200200",
        Name = "St. Gallen",
    };

    /// <summary>
    /// A default mocked tenant called Gossau.
    /// </summary>
    public static readonly Tenant MockedTenantGossau = new Tenant
    {
        Id = "300300300300300300",
        Name = "Gossau",
    };

    /// <summary>
    /// A default mocked tenant called Genf.
    /// </summary>
    public static readonly Tenant MockedTenantGenf = new Tenant
    {
        Id = "400400400400400400",
        Name = "Genf",
    };

    /// <summary>
    /// A default mocked tenant called Bund.
    /// </summary>
    public static readonly Tenant MockedTenantBund = new Tenant
    {
        Id = "999999999999999999",
        Name = "Bund",
    };

    /// <summary>
    /// Returns a list of all mocked tenants.
    /// </summary>
    public static readonly List<Tenant> MockedTenants = new List<Tenant>
        {
            MockedTenantDefault,
            MockedTenantUzwil,
            MockedTenantStGallen,
            MockedTenantGossau,
            MockedTenantGenf,
            MockedTenantBund,
        };

    /// <summary>
    /// A default mocked user.
    /// </summary>
    public static readonly User MockedUserDefault = new User
    {
        Loginid = "default-user-id",
        Firstname = "default user firstname",
        Lastname = "default user lastname",
        Username = "default username",
    };

    /// <summary>
    /// A mocked service user. Service users do not provide a first or last name.
    /// </summary>
    public static readonly User MockedUserService = new User
    {
        Loginid = "service-user-id",
        Firstname = null,
        Lastname = null,
        Username = "service-user",
        Servicename = "service-name",
    };

    /// <summary>
    /// Returns a list of all mocked users.
    /// </summary>
    public static readonly List<User> MockedUsers = new List<User>
        {
            MockedUserDefault,
            MockedUserService,
        };

    /// <summary>
    /// A mocked verified 2fa identifier.
    /// </summary>
    public static readonly string MockedVerified2faId = "21092903-acd5-4630-8c86-2209647d9517";
}
