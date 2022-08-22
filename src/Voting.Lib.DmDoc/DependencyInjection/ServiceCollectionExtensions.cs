// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Threading;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.DmDoc;
using Voting.Lib.DmDoc.Configuration;
using Voting.Lib.DmDoc.Serialization;
using Voting.Lib.DmDoc.Serialization.Json;
using Voting.Lib.DmDoc.Serialization.Xml;

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
        services.AddHttpClient<IDmDocService, DmDocService>(c =>
        {
            c.Timeout = config.Timeout ?? Timeout.InfiniteTimeSpan;
            c.BaseAddress = config.BaseAddress
                ?? throw new ValidationException("DmDoc base address is required");

            if (string.IsNullOrWhiteSpace(config.Token))
            {
                throw new ValidationException("DmDoc token is required");
            }

            c.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(config.Token);
        });
        services.TryAddSingleton(config);
        services.TryAddTransient<IDmDocUrlBuilder, DmDocUrlBuilder>();
        services.TryAddTransient<IDmDocUserNameProvider, DmDocConfigUserNameProvider>();

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
