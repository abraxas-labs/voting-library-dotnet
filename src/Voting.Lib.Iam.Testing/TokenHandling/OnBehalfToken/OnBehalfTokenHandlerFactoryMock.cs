// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.Iam.TokenHandling;
using Voting.Lib.Iam.TokenHandling.OnBehalfToken;

namespace Voting.Lib.Iam.Testing.TokenHandling.OnBehalfToken;

/// <summary>
/// Mock for <see cref="IOnBehalfTokenHandlerFactory"/>.
/// </summary>
public class OnBehalfTokenHandlerFactoryMock : IOnBehalfTokenHandlerFactory
{
    /// <summary>
    /// Creates a <see cref="OnBehalfTokenHandlerMock"/>.
    /// </summary>
    /// <param name="configName">The config name (ignored).</param>
    /// <param name="serviceAccountConfigName">The service account config name (ignored).</param>
    /// <param name="clientName">The client name (ignored).</param>
    /// <returns>Returns a <see cref="OnBehalfTokenHandlerMock"/>.</returns>
    public ITokenHandler CreateHandler(string configName, string serviceAccountConfigName, string clientName)
        => new OnBehalfTokenHandlerMock();
}
