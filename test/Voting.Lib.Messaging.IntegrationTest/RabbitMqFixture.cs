// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.RabbitMq;
using Voting.Lib.Messaging.Configuration;
using Xunit;

namespace Voting.Lib.Messaging.IntegrationTest;

public class RabbitMqFixture : IAsyncLifetime
{
    private RabbitMqContainer _rabbitMqContainer = null!; // initialized during InitializeAsync

    public ServiceProvider ServiceProvider { get; private set; } = null!; // initialized during InitializeAsync

    public RabbitMqConfig Configuration { get; private set; } = null!; // initialized during InitializeAsync

    public virtual async Task InitializeAsync()
    {
        _rabbitMqContainer = await RabbitMqTestContainer.StartNew();

        Configuration = new()
        {
            Hosts = new() { "localhost:" + _rabbitMqContainer.GetMappedPublicPort(RabbitMqTestContainer.ApiPort) },
            Username = RabbitMqTestContainer.Username,
            Password = RabbitMqTestContainer.Password,
        };

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider(true);
        await ServiceProvider.StartHostedServices();
        await ServiceProvider.GetRequiredService<IBusControl>().StartAsync();
    }

    public async Task DisposeAsync()
    {
        await ServiceProvider.StopHostedServices();
        await ServiceProvider.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
    }

    protected virtual void ConfigureServices(ServiceCollection services)
    {
        services.AddVotingLibMessaging(Configuration, ConfigureMessaging);
    }

    protected virtual void ConfigureMessaging(IServiceCollectionBusConfigurator bus)
    {
    }
}
