// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Linq;
using Voting.Lib.ProtoValidation;
using Voting.Lib.ProtoValidation.Validators;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for proto validators.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the services related to proto validation to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddProtoValidators(this IServiceCollection services)
    {
        var protoValidatorType = typeof(IProtoFieldValidator);

        var types = protoValidatorType.Assembly.GetTypes()
            .Where(t => protoValidatorType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();

        foreach (var type in types)
        {
            services.Add(new ServiceDescriptor(protoValidatorType, type, ServiceLifetime.Singleton));
        }

        services.AddSingleton<ProtoValidator>();

        return services;
    }
}
