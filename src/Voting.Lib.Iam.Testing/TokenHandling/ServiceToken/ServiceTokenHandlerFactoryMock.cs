// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Iam.TokenHandling;
using Voting.Lib.Iam.TokenHandling.ServiceToken;

namespace Voting.Lib.Iam.Testing.TokenHandling.ServiceToken;

/// <summary>
/// Mock for <see cref="IServiceTokenHandlerFactory"/>.
/// </summary>
public class ServiceTokenHandlerFactoryMock : IServiceTokenHandlerFactory
{
    /// <summary>
    /// Creates a <see cref="ServiceTokenHandlerMock"/>.
    /// </summary>
    /// <param name="configName">The config name (ignored).</param>
    /// <returns>Returns a <see cref="ServiceTokenHandlerMock"/>.</returns>
    public ITokenHandler CreateHandler(string configName)
        => new ServiceTokenHandlerMock();
}
