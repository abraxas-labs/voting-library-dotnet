// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.ImageProcessing;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for image processing.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the services related to image processing to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddImageProcessing(this IServiceCollection services)
    {
        return services.AddSingleton<IImageProcessor, MagickImageProcessor>();
    }
}
