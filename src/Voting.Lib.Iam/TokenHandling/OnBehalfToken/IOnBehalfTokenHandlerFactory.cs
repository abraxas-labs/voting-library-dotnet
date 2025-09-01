// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Iam.TokenHandling.ServiceToken;

namespace Voting.Lib.Iam.TokenHandling.OnBehalfToken;

/// <summary>
/// Factory to create <see cref="ITokenHandler"/> instances.
/// </summary>
public interface IOnBehalfTokenHandlerFactory
{
    /// <summary>
    /// Creates a new <see cref="ITokenHandler"/>.
    /// </summary>
    /// <param name="configName">The <see cref="SecureConnectOnBehalfOptions"/> name.</param>
    /// <param name="serviceAccountConfigName">The <see cref="SecureConnectServiceAccountOptions"/> name.</param>
    /// <param name="clientName">The name of the http client to use to fetch the ob_token.</param>
    /// <returns>The created handler.</returns>
    ITokenHandler CreateHandler(string configName, string serviceAccountConfigName, string clientName);
}
