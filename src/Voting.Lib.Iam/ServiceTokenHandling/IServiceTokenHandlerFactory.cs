// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Iam.ServiceTokenHandling;

/// <summary>
/// Factory to create <see cref="IServiceTokenHandler"/> instances.
/// </summary>
public interface IServiceTokenHandlerFactory
{
    /// <summary>
    /// Creates a new <see cref="IServiceTokenHandler"/>.
    /// </summary>
    /// <param name="configName">The <see cref="SecureConnectServiceAccountOptions"/> name.</param>
    /// <returns>The created handler.</returns>
    IServiceTokenHandler CreateHandler(string configName);
}
