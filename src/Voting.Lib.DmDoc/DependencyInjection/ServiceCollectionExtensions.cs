// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.DmDoc;
using Voting.Lib.DmDoc.Configuration;
using Voting.Lib.DmDoc.Serialization;
using Voting.Lib.DmDoc.Serialization.Json;
using Voting.Lib.DmDoc.Serialization.Xml;
using Voting.Lib.Scheduler;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for DmDoc.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the DmDoc services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The DmDoc configuration.</param>
    /// <returns>Returns the service collection.</returns>
    /// <exception cref="ValidationException">Thrown if the DmDoc configuration is invalid.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the DmDoc serialization format is invalid.</exception>
    public static IServiceCollection AddDmDoc(this IServiceCollection services, DmDocConfig config)
    {
        services.TryAddSingleton(config);
        services.TryAddSingleton<IDmDocDraftCleanupQueue, DmDocDraftCleanupQueue>();

        services.TryAddTransient<IDmDocService, DmDocService>();
        services.TryAddTransient<IDmDocUrlBuilder, DmDocUrlBuilder>();
        services.TryAddTransient<IDmDocUserNameProvider, DmDocConfigUserNameProvider>();

        services.AddScheduledJob<DmDocDraftCleanupJob>(config.DraftCleanupScheduler);

        switch (config.DataSerializationFormat)
        {
            case DmDocDataSerializationFormat.Json:
                services.TryAddSingleton<IDmDocDataSerializer, DmDocJsonDataSerializer>();
                break;
            case DmDocDataSerializationFormat.Xml:
                services.TryAddSingleton<IDmDocDataSerializer, DmDocXmlDataSerializer>();
                break;
            default:
                throw new ValidationException("The serialization format " + config.DataSerializationFormat + " is not supported");
        }

        return services;
    }
}
