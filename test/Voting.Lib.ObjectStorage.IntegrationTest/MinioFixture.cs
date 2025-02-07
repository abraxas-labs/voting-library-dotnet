// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Minio;
using Voting.Lib.ObjectStorage.Config;
using Voting.Lib.ObjectStorage.DependencyInjection;
using Xunit;

namespace Voting.Lib.ObjectStorage.IntegrationTest;

public class MinioFixture : IAsyncLifetime
{
    private const string MinioImage = "docker.io/minio/minio:RELEASE.2024-04-18T19-09-19Z";
    private const int MinioApiPort = 9000;

    private MinioContainer _minioContainer = null!; // initialized during InitializeAsync

    public ServiceProvider ServiceProvider { get; private set; } = null!; // initialized during InitializeAsync

    public ObjectStorageConfig Configuration { get; private set; } = null!; // initialized during InitializeAsync

    public virtual async Task InitializeAsync()
    {
        _minioContainer = new MinioBuilder()
            .WithImage(MinioImage)
            .Build();

        await _minioContainer.StartAsync();
        Configuration = new()
        {
            Endpoint = "localhost:" + _minioContainer.GetMappedPublicPort(MinioApiPort),
            UseSsl = false,
            AccessKey = _minioContainer.GetAccessKey(),
            SecretKey = _minioContainer.GetSecretKey(),
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
