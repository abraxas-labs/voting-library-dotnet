// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.EventStoreDb;
using Voting.Lib.Eventing.Configuration;
using Voting.Lib.Eventing.DependencyInjection;
using Voting.Lib.Eventing.IntegrationTest.Events;
using Xunit;

namespace Voting.Lib.Eventing.IntegrationTest;

public class EventStoreFixture : IAsyncLifetime
{
    private EventStoreDbContainer _eventStoreContainer = null!; // initialized during InitializeAsync

    public ServiceProvider ServiceProvider { get; private set; } = null!; // initialized during InitializeAsync

    public EventStoreConfig Configuration { get; private set; } = null!; // initialized during InitializeAsync

    public virtual async Task InitializeAsync()
    {
        _eventStoreContainer = await EventStoreTestContainer.StartNew();
        Configuration = new()
        {
            Authorities = new() { _eventStoreContainer.Hostname + ":" + _eventStoreContainer.GetMappedPublicPort(EventStoreTestContainer.HttpPort) },
            UseTls = false,
        };

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider(true);
    }

    public virtual async Task DisposeAsync()
    {
        await ServiceProvider.DisposeAsync();
        await _eventStoreContainer.DisposeAsync();
    }

    public T GetService<T>()
        where T : notnull
        => ServiceProvider.GetRequiredService<T>();

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        ConfigureEventing(services.AddVotingLibEventing(Configuration, typeof(TestEvent).Assembly).AddMetadataDescriptorProvider<TestMetadataProvider>());
    }

    protected virtual void ConfigureEventing(IEventingServiceCollection eventing)
    {
    }
}
