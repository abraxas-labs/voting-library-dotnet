// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.Iam.Testing.AuthenticationScheme;
using Voting.Lib.Iam.TokenHandling;

namespace Voting.Lib.Iam.Testing.TokenHandling.ServiceToken;

/// <summary>
/// A mock for the service token handler which always returns a static service token.
/// </summary>
public class ServiceTokenHandlerMock : ITokenHandler
{
    /// <inheritdoc />
    public Task<string> GetToken(CancellationToken cancellationToken)
        => Task.FromResult(SecureConnectTestDefaults.ServiceToken);
}
