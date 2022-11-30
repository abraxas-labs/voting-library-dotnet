// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using EventStore.Client;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Eventing.Configuration;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Persistence;
using Voting.Lib.Eventing.Protobuf;
using Voting.Lib.Eventing.Read;
using Voting.Lib.Eventing.Seeding;
using Voting.Lib.Eventing.Subscribe;
using Voting.Lib.Scheduler;

namespace Voting.Lib.Eventing.DependencyInjection;

/// <inheritdoc />
public class EventingServiceCollection : IEventingServiceCollection
{
    private readonly EventStoreConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventingServiceCollection"/> class.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The EventStore configuration.</param>
    /// <param name="protoAssemblies">List of assemblies where protobuf definitions reside.</param>
    public EventingServiceCollection(IServiceCollection services, EventStoreConfig config, IEnumerable<Assembly> protoAssemblies)
    {
        Services = services;
        _config = config;
        AddCore(config, protoAssemblies);
    }

    /// <inheritdoc cref="IEventingServiceCollection"/>
    public IServiceCollection Services { get; }

    /// <inheritdoc cref="IEventingServiceCollection"/>
    public IEventingServiceCollection AddPublishing()
    {
        Services.TryAddSingleton<IEventPublisher, EventPublisher>();
        Services.TryAddSingleton<IAggregateEventReader, AggregateEventReader>();
        Services.TryAddScoped<IAggregateRepository, AggregateRepository>();
        Services.TryAddScoped<IAggregateRepositoryHandler, AggregateRepositoryHandler>();
        Services.TryAddScoped<IAggregateFactory, AggregateFactory>();
        Services.TryAddSingleton<IEventSeeder, EventSeeder>();
        Services.AddHostedService<AggregateSeeder>();
        return this;
    }

    /// <inheritdoc cref="IEventingServiceCollection"/>
    public IEventingServiceCollection AddPublishing<T>()
        where T : BaseEventSourcingAggregate
    {
        AddPublishing();
        AddAggregatesFromAssemblyOfType<T>();
        AddAggregateSeedersFromAssemblyOfType<T>();
        return this;
    }

    /// <inheritdoc cref="IEventingServiceCollection"/>
    public IEventingServiceCollection AddTransientSubscription<TAssembly>(string streamName)
    {
        Services.AddSingleton<TransientEventProcessorScope>();
        return AddSubscription<TransientEventProcessorScope, TAssembly>(streamName);
    }

    /// <inheritdoc cref="IEventingServiceCollection"/>
    public IEventingServiceCollection AddSubscription<TScope>(string streamName)
        where TScope : class, IEventProcessorScope
        => AddSubscription<TScope, TScope>(streamName);

    /// <inheritdoc cref="IEventingServiceCollection"/>
    public IEventingServiceCollection AddSubscription<TScope, TAssembly>(string streamName)
        where TScope : class, IEventProcessorScope
    {
        Services.TryAddSingleton<IEventProcessingRetryPolicy<TScope>, EventProcessingRetryPolicy<TScope>>();
        Services.AddScheduledJob<LatestEventMonitor>(_config.LatestEventPositionMonitorInterval, true);
        Services.AddHostedService<EventProcessingSubscriber<TScope>>();
        Services.TryAddSingleton<EventProcessingHandler<TScope>>();
        Services.TryAddSingleton<EventProcessorAdapterRegistry<TScope>>();
        Services.TryAddScoped<TScope>();
        Services.AddScoped<IEventProcessorScope>(sp => sp.GetRequiredService<TScope>());

        var eventTypes = AddEventProcessors<TScope, TAssembly>();
        var subscription = new Subscription<TScope>(eventTypes, streamName);
        Services.AddSingleton(subscription);
        Services.AddSingleton<ISubscription>(subscription);

        return this;
    }

    private static bool HasEventProcessorInterface(Type t, Type eventProcessorScopeType)
        => t.GetInterfaces().Any(x => IsEventProcessor(x, eventProcessorScopeType));

    private static bool IsEventProcessor(Type t, Type eventProcessorScopeType)
        => t.IsGenericType
            && t.GetGenericTypeDefinition() == typeof(ICatchUpDetectorEventProcessor<,>)
            && t.GetGenericArguments()[0] == eventProcessorScopeType;

    private void AddAggregateSeedersFromAssemblyOfType<T>()
    {
        Services.Scan(scan => scan.FromAssemblyOf<T>()
            .AddClasses(classes => classes.AssignableTo<IAggregateSeedSource>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    private void AddAggregatesFromAssemblyOfType<T>()
    {
        Services.Scan(scan => scan.FromAssemblyOf<T>()
            .AddClasses(classes => classes.AssignableTo(typeof(BaseEventSourcingAggregate)))
            .AsSelf()
            .WithTransientLifetime());
    }

    private IReadOnlyCollection<Type> AddEventProcessors<TScope, TAssembly>()
        where TScope : IEventProcessorScope
    {
        Services.Scan(scan => scan.FromAssemblyOf<TAssembly>()
            .AddClasses(classes => classes.Where(x => HasEventProcessorInterface(x, typeof(TScope))))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // can't use open generic to register the event processor adapter since we only want to process
        // events for which an event processor exists
        // sadly it is also not possible to do this with scrutor
        var eventTypes = Services
            .Where(s => IsEventProcessor(s.ServiceType, typeof(TScope)))
            .Select(x => x.ServiceType.GetGenericArguments()[1])
            .ToHashSet();

        var adapterTypes = eventTypes
            .Select(et => typeof(EventProcessorAdapter<,>).MakeGenericType(typeof(TScope), et));

        foreach (var adapter in adapterTypes)
        {
            Services.TryAddScoped(adapter);
        }

        return eventTypes;
    }

    private void AddCore(
        EventStoreConfig config,
        IEnumerable<Assembly> protoAssemblies)
    {
        Services.AddHttpClient();
        Services.TryAddSingleton(config);
        Services.TryAddSingleton<IEventSerializer, ProtobufJsonSerializer>();
        Services.TryAddSingleton(JsonFormatter.Default);
        Services.TryAddSingleton<IProtobufTypeRegistry>(ProtobufTypeRegistry.CreateByScanningAssemblies(protoAssemblies));
        Services.TryAddSingleton(new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true)));
        Services.TryAddSingleton(static sp => new EventStoreClient(sp.GetRequiredService<EventStoreClientSettings>()));
        Services.TryAddSingleton<IEventReader, EventReader>();
        Services.TryAddSingleton<EventTypeResolver>();
        Services.TryAddSingleton(sp =>
        {
            var eventStoreClientSettings = EventStoreClientSettings.Create(config.ConnectionString);
            eventStoreClientSettings.OperationOptions = EventStoreClientOperationOptions.Default.Clone();
            eventStoreClientSettings.OperationOptions.TimeoutAfter = config.OperationTimeout;
            eventStoreClientSettings.CreateHttpMessageHandler = () =>
                sp.GetRequiredService<IHttpMessageHandlerFactory>().CreateHandler(config.HttpClientName);
            config.Customize(eventStoreClientSettings);
            return eventStoreClientSettings;
        });
    }
}
