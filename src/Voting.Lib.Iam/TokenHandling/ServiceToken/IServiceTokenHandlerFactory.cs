// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.TokenHandling.ServiceToken;

/// <summary>
/// Factory to create <see cref="ITokenHandler"/> instances.
/// </summary>
public interface IServiceTokenHandlerFactory
{
    /// <summary>
    /// Creates a new <see cref="ITokenHandler"/>.
    /// </summary>
    /// <param name="configName">The <see cref="SecureConnectServiceAccountOptions"/> name.</param>
    /// <returns>The created handler.</returns>
    ITokenHandler CreateHandler(string configName);
}
