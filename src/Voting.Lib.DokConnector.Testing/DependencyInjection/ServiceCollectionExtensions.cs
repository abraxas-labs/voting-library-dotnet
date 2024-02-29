// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.DokConnector.Service;
using Voting.Lib.DokConnector.Testing.Service;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for DOK connector mocks.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Replaces all <see cref="IDokConnector"/> with a mock instance.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The http client builder.</returns>
    public static IServiceCollection AddDokConnectorMock(this IServiceCollection services)
    {
        return services
            .RemoveAll<IDokConnector>()
            .AddForwardRefSingleton<IDokConnector, DokConnectorMock>();
    }
}
