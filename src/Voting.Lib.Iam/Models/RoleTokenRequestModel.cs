// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Iam.Models;

/// <summary>
/// The role token request model which is getting serialized and sent to the token endpoint.
/// </summary>
internal class RoleTokenRequestModel
{
    internal RoleTokenRequestModel(string subjectToken, ICollection<string>? apps)
    {
        SubjectToken = subjectToken;
        Apps = apps;
    }

    public string GrantType => SecureConnectGrantTypes.GrantTypeRoleToken;

    public string SubjectToken { get; }

    public ICollection<string>? Apps { get; }
}
