// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.Models;

/// <summary>
/// The on behalf token request model which is getting serialized and sent to the token endpoint.
/// </summary>
internal class OnBehalfTokenRequestModel
{
    internal OnBehalfTokenRequestModel(string subjectToken, string resource)
    {
        SubjectToken = subjectToken;
        Resource = resource;
    }

    public string GrantType => SecureConnectGrantTypes.GrantTypeOnBehalfToken;

    public string SubjectToken { get; }

    public string Resource { get; }
}
