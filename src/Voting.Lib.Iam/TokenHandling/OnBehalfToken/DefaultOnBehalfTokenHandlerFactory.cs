// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Concurrent;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Voting.Lib.Iam.TokenHandling.ServiceToken;

namespace Voting.Lib.Iam.TokenHandling.OnBehalfToken;

internal class DefaultOnBehalfTokenHandlerFactory : IOnBehalfTokenHandlerFactory
{
    private static readonly ObjectFactory OnBehalfTokenHandlerFactory = ActivatorUtilities.CreateFactory(
        typeof(OnBehalfTokenHandler),
        [typeof(SecureConnectOnBehalfOptions), typeof(SecureConnectServiceAccountOptions), typeof(HttpClient)]);

    private readonly IOptionsMonitor<SecureConnectOnBehalfOptions> _options;
    private readonly IOptionsMonitor<SecureConnectServiceAccountOptions> _serviceAccountOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<(string ConfigName, string ServiceAccountConfigName), ITokenHandler> _onBehalfTokenHandlers = new();

    public DefaultOnBehalfTokenHandlerFactory(
        IOptionsMonitor<SecureConnectOnBehalfOptions> options,
        IOptionsMonitor<SecureConnectServiceAccountOptions> serviceAccountOptions,
        IServiceProvider serviceProvider)
    {
        _options = options;
        _serviceProvider = serviceProvider;
        _serviceAccountOptions = serviceAccountOptions;
    }

    public ITokenHandler CreateHandler(string configName, string serviceAccountConfigName, string clientName)
    {
        return _onBehalfTokenHandlers.GetOrAdd((configName, serviceAccountConfigName), _ =>
        {
            var options = _options.Get(configName);
            var serviceAccountOptions = _serviceAccountOptions.Get(serviceAccountConfigName);
            var httpClient = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(clientName);

            // Create an ob_token handler,
            // that uses the service-token authenticated http client to fetch the ob_token.
            // The service account options are used to resolve the same oidc-config as the service account uses.
            return (ITokenHandler)OnBehalfTokenHandlerFactory(_serviceProvider, [options, serviceAccountOptions, httpClient]);
        });
    }
}
