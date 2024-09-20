// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Voting.Lib.ObjectStorage.Config;
using Voting.Lib.ObjectStorage.DependencyInjection;

namespace Voting.Lib.ObjectStorage.IntegrationTest;

public class MinioSampleDataFixture : MinioFixture
{
    protected SampleBucketClient BucketClient { get; private set; } = null!; // initialized during InitializeAsync

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        BucketClient = ServiceProvider.GetRequiredService<SampleBucketClient>();
        await BucketClient.EnsureBucketExists();

        for (var i = 0; i < 10; i++)
        {
            await using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes($"test-file-content-{i}"));
            await BucketClient.Store(ObjectName(i), memoryStream, "plain/text");
        }
    }

    protected override void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton(new ObjectStorageBucketConfig { BucketName = "test-bucket", ObjectPrefix = "test" });
        base.ConfigureServices(services);
    }

    protected override void ConfigureObjectStorage(ObjectStorageClientBuilder builder)
    {
        base.ConfigureObjectStorage(builder);
        builder.AddBucketClient<SampleBucketClient>();
    }

    protected string ObjectName(int i)
        => $"test-file-{i}.txt";
}
