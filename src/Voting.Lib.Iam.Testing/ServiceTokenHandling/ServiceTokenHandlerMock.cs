// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Voting.Lib.Iam.ServiceTokenHandling;
using Voting.Lib.Iam.Testing.AuthenticationScheme;

namespace Voting.Lib.Iam.Testing.ServiceTokenHandling;

/// <summary>
/// A mock for the service token handler which always returns a static service token.
/// </summary>
public class ServiceTokenHandlerMock : IServiceTokenHandler
{
    /// <inheritdoc />
    public Task<string> GetServiceToken()
        => Task.FromResult(SecureConnectTestDefaults.ServiceToken);
}
