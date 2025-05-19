// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.Eventing.Domain;
using Voting.Lib.Eventing.Persistence;
using Voting.Lib.Eventing.Subscribe;

namespace Voting.Lib.Eventing.DependencyInjection;

/// <summary>
/// A eventing service collection which can be used to add eventing specific services.
/// </summary>
public interface IEventingServiceCollection
{
    /// <summary>
    /// Gets the service collection.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Adds event publishing to the services without any aggregates.
    /// </summary>
    /// <returns>The <see cref="IEventingServiceCollection"/> instance.</returns>
    IEventingServiceCollection AddPublishing();

    /// <summary>
    /// Adds event publishing to the services.
    /// Scans for all <see cref="BaseEventSourcingAggregate"/> and <see cref="Voting.Lib.Eventing.Seeding.IAggregateSeedSource"/> implementations.
    /// </summary>
    /// <typeparam name="T">The type of the assembly to scan for aggregates.</typeparam>
    /// <returns>The <see cref="IEventingServiceCollection"/> instance.</returns>
    IEventingServiceCollection AddPublishing<T>()
        where T : BaseEventSourcingAggregate;

    /// <summary>
    /// Registers event processors of the transient scope and creates a subscription for the transient scope.
    /// A transient subscription, is a subscription which always starts processing events from the very beginning
    /// and has no persisted read model (also see <see cref="TransientEventProcessorScope"/>).
    /// Only one transient subscription is supported.
    /// </summary>
    /// <param name="streamName">The name of the stream to subscribe.</param>
    /// <typeparam name="TAssembly">Assembly of this type is used for assembly scanning to find event processors.</typeparam>
    /// <returns>The <see cref="IEventingServiceCollection"/> instance.</returns>
    IEventingServiceCollection AddTransientSubscription<TAssembly>(string streamName);

    /// <summary>
    /// Registers event processors of the provided scope and creates a subscription for this scope.
    /// </summary>
    /// <param name="streamName">The name of the stream to subscribe.</param>
    /// <typeparam name="TScope">
    /// The event processing scope implementation type.
    /// The assembly of this type is used for assembly scanning to detect the EventProcessors.
    /// </typeparam>
    /// <returns>The <see cref="IEventingServiceCollection"/> instance.</returns>
    IEventingServiceCollection AddSubscription<TScope>(string streamName)
        where TScope : class, IEventProcessorScope;

    /// <summary>
    /// Registers event processors of the provided scope and creates a subscription for this scope.
    /// </summary>
    /// <param name="streamName">The name of the stream to subscribe.</param>
    /// <typeparam name="TScope">
    /// The event processing scope implementation type.
    /// </typeparam>
    /// <typeparam name="TAssembly">Assembly of this type is used for assembly scanning to find event processors.</typeparam>
    /// <returns>The <see cref="IEventingServiceCollection"/> instance.</returns>
    IEventingServiceCollection AddSubscription<TScope, TAssembly>(string streamName)
        where TScope : class, IEventProcessorScope;

    /// <summary>
    /// Adds a metadata descriptor provider.
    /// </summary>
    /// <typeparam name="TResolver">The type of resolver.</typeparam>
    /// <returns>The <see cref="IEventingServiceCollection"/> instance.</returns>
    IEventingServiceCollection AddMetadataDescriptorProvider<TResolver>()
        where TResolver : class, IMetadataDescriptorProvider;
}
