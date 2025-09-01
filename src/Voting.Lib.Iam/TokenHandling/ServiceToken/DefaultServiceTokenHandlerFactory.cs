// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Voting.Lib.Iam.TokenHandling.ServiceToken;

internal class DefaultServiceTokenHandlerFactory : IServiceTokenHandlerFactory
{
    private static readonly ObjectFactory ServiceTokenHandlerFactory = ActivatorUtilities.CreateFactory(
        typeof(ServiceTokenHandler),
        [typeof(SecureConnectServiceAccountOptions)]);

    private readonly IOptionsMonitor<SecureConnectServiceAccountOptions> _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, ITokenHandler> _serviceTokenHandlers = new();

    public DefaultServiceTokenHandlerFactory(
        IOptionsMonitor<SecureConnectServiceAccountOptions> options,
        IServiceProvider serviceProvider)
    {
        _options = options;
        _serviceProvider = serviceProvider;
    }

    public ITokenHandler CreateHandler(string configName)
    {
        return _serviceTokenHandlers.GetOrAdd(configName, _ =>
        {
            var options = _options.Get(configName);
            return (ITokenHandler)ServiceTokenHandlerFactory(_serviceProvider, [options]);
        });
    }
}
