// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.ObjectStorage.Config;
using Voting.Lib.ObjectStorage.DependencyInjection;
using Xunit;

namespace Voting.Lib.ObjectStorage.IntegrationTest;

public class MinioFixture : IAsyncLifetime
{
    private TestcontainersContainer _minioContainer = null!; // initialized during InitializeAsync

    public ServiceProvider ServiceProvider { get; private set; } = null!; // initialized during InitializeAsync

    public ObjectStorageConfig Configuration { get; private set; } = null!; // initialized during InitializeAsync

    public virtual async Task InitializeAsync()
    {
        _minioContainer = MinioTestContainer.Build();

        await _minioContainer.StartAsync();
        Configuration = new()
        {
            Endpoint = "localhost:" + _minioContainer.GetMappedPublicPort(MinioTestContainer.ApiPort),
            UseSsl = false,
            AccessKey = MinioTestContainer.Username,
            SecretKey = MinioTestContainer.Password,
        };

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider(true);
    }

    public async Task DisposeAsync()
    {
        await ServiceProvider.DisposeAsync();
        await _minioContainer.DisposeAsync();
    }

    protected virtual void ConfigureServices(ServiceCollection services)
    {
        ConfigureObjectStorage(services.AddVotingLibObjectStorage(Configuration));
    }

    protected virtual void ConfigureObjectStorage(ObjectStorageClientBuilder builder)
    {
    }
}
