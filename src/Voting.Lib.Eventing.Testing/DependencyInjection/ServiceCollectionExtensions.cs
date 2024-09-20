// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Eventing.Persistence;
using Voting.Lib.Eventing.Read;
using Voting.Lib.Eventing.Seeding;
using Voting.Lib.Eventing.Testing.Mocks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extension for adding eventing mocks.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Removes/replaces all eventing services with mocks.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddVotingLibEventingMocks(this IServiceCollection services)
        => services
            .AddSingleton<TestEventPublisher>()
            .RemoveAll<IEventPublisher>()
            .AddSingleton<EventPublisherMock>()
            .AddSingleton<IEventPublisher>(sp => sp.GetRequiredService<EventPublisherMock>())
            .RemoveAll<IAggregateRepository>()
            .AddSingleton<AggregateRepositoryMockStore>()
            .AddScoped<AggregateRepositoryMock>()
            .AddScoped<IAggregateRepository>(sp => sp.GetRequiredService<AggregateRepositoryMock>())
            .RemoveAll<IEventSeeder>()
            .RemoveAll<IEventReader>()
            .AddScoped<EventReaderMock>()
            .AddScoped<IEventReader>(sp => sp.GetRequiredService<EventReaderMock>())
            .AddSingleton<EventReaderMockStore>();
}
