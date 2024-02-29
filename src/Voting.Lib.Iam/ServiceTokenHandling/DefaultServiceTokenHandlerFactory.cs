// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Voting.Lib.Iam.ServiceTokenHandling;

internal class DefaultServiceTokenHandlerFactory : IServiceTokenHandlerFactory
{
    private static readonly ObjectFactory ServiceTokenHandlerFactory = ActivatorUtilities.CreateFactory(
        typeof(ServiceTokenHandler),
        new[] { typeof(SecureConnectServiceAccountOptions) });

    private readonly IOptionsMonitor<SecureConnectServiceAccountOptions> _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, IServiceTokenHandler> _serviceTokenHandlers = new();

    public DefaultServiceTokenHandlerFactory(
        IOptionsMonitor<SecureConnectServiceAccountOptions> options,
        IServiceProvider serviceProvider)
    {
        _options = options;
        _serviceProvider = serviceProvider;
    }

    public IServiceTokenHandler CreateHandler(string configName)
    {
        return _serviceTokenHandlers.GetOrAdd(configName, _ =>
        {
            var options = _options.Get(configName);
            return (IServiceTokenHandler)ServiceTokenHandlerFactory(_serviceProvider, new object?[] { options });
        });
    }
}
