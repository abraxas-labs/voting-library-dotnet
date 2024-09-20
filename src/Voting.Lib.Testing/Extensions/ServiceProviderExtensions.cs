// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceProvider"/>.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Starts all <see cref="IHostedService"/>.
    /// </summary>
    /// <param name="sp">The service provider.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task StartHostedServices(this IServiceProvider sp)
    {
        foreach (var service in sp.GetRequiredService<IEnumerable<IHostedService>>())
        {
            await service.StartAsync(CancellationToken.None);
        }
    }

    /// <summary>
    /// Stops all <see cref="IHostedService"/>.
    /// </summary>
    /// <param name="sp">The service provider.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task StopHostedServices(this IServiceProvider sp)
    {
        foreach (var service in sp.GetRequiredService<IEnumerable<IHostedService>>())
        {
            await service.StopAsync(CancellationToken.None);
        }
    }
}
