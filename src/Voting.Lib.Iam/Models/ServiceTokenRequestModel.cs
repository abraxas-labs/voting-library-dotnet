// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Iam.Models;

/// <summary>
/// The service token request model which is getting serialized and sent to the token endpoint.
/// </summary>
internal class ServiceTokenRequestModel
{
    internal ServiceTokenRequestModel(string clientId, string clientSecret)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
    }

    public string GrantType => SecureConnectGrantTypes.GrantTypeClientCredentials;

    public string ClientId { get; }

    public string ClientSecret { get; }

    public IReadOnlyCollection<string>? Scope { get; set; }
}
