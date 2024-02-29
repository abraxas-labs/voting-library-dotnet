// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Minio.Exceptions;
using Voting.Lib.ObjectStorage.Config;
using Xunit;

namespace Voting.Lib.ObjectStorage.IntegrationTest;

public class BucketObjectStorageClientTest : MinioSampleDataFixture
{
    [Fact]
    public async Task FetchShouldWork()
    {
        var receivedString = string.Empty;
        await BucketClient.Fetch(ObjectName(1), content =>
        {
            using var sw = new StreamReader(content);
            receivedString = sw.ReadToEnd();
        });

        receivedString.Should().Be("test-file-content-1");
    }

    [Fact]
    public Task FetchShouldThrowForUnknown()
    {
        return Assert.ThrowsAsync<ObjectNotFoundException>(async () => await BucketClient.Fetch(ObjectName(-1), _ => { }));
    }

    [Fact]
    public async Task FetchAsBase64StreamShouldWork()
    {
        var receivedString = string.Empty;
        await BucketClient.FetchAsBase64(ObjectName(1), content =>
        {
            using var sw = new StreamReader(content);
            receivedString = sw.ReadToEnd();
        });

        receivedString.Should().Be(Convert.ToBase64String(Encoding.UTF8.GetBytes("test-file-content-1")));
    }

    [Fact]
    public Task FetchAsBase64StreamShouldThrowForUnknown()
    {
        return Assert.ThrowsAsync<ObjectNotFoundException>(async () => await BucketClient.FetchAsBase64(ObjectName(-1), _ => { }));
    }

    [Fact]
    public async Task FetchAsBase64StringShouldWork()
    {
        var receivedString = await BucketClient.FetchAsBase64(ObjectName(1));
        receivedString.Should().Be(Convert.ToBase64String(Encoding.UTF8.GetBytes("test-file-content-1")));
    }

    [Fact]
    public Task FetchAsBase64StringShouldThrowForUnknown()
    {
        return Assert.ThrowsAsync<ObjectNotFoundException>(async () => await BucketClient.FetchAsBase64(ObjectName(-1)));
    }

    [Fact]
    public async Task DeleteShouldWork()
    {
        await BucketClient.Delete(ObjectName(1));
        await Assert.ThrowsAsync<ObjectNotFoundException>(async () => await BucketClient.FetchAsBase64(ObjectName(1)));
    }

    [Fact]
    public async Task GetPublicDownloadUrlShouldWork()
    {
        var objectName = ObjectName(1);
        var uri = await BucketClient.GetPublicDownloadUrl(objectName);
        var strUri = uri.ToString();
        strUri.Should().Contain(objectName);
        strUri.Should().StartWith("http://localhost");
        strUri.Should().Contain("response-content-disposition=attachment");
        strUri.Should().Contain("X-Amz-Signature");
    }

    [Fact]
    public async Task GetPublicDownloadUrlWithOverwrittenDefaultParametersShouldWork()
    {
        await using var serviceProvider = new ServiceCollection()
            .AddSingleton(new ObjectStorageBucketConfig
            {
                BucketName = "test-bucket",
                ObjectPrefix = "prefix",
                DefaultPublicDownloadLinkTtl = TimeSpan.FromMinutes(1),
                DefaultPublicDownloadLinkParameters = new()
                {
                    ["my-param"] = "my-param-value",
                },
            })
            .AddVotingLibObjectStorage(Configuration)
            .AddBucketClient<SampleBucketClient>()
            .Services
            .BuildServiceProvider(true);

        var client = serviceProvider.GetRequiredService<SampleBucketClient>();
        var objectName = ObjectName(1);
        var uri = await client.GetPublicDownloadUrl(objectName);
        var strUri = uri.ToString();
        strUri.Should().Contain(objectName);
        strUri.Should().StartWith("http://localhost");
        strUri.Should().NotContain("response-content-disposition=attachment");
        strUri.Should().Contain("my-param=my-param-value");
        strUri.Should().Contain("X-Amz-Signature");
    }

    [Fact]
    public async Task GetPublicDownloadUrlShouldWorkForUnknown()
    {
        var objectName = ObjectName(-1);
        var uri = await BucketClient.GetPublicDownloadUrl(objectName);
        var strUri = uri.ToString();
        strUri.Should().Contain(objectName);
        strUri.Should().StartWith("http://localhost");
        strUri.Should().Contain("response-content-disposition=attachment");
        strUri.Should().Contain("X-Amz-Signature");
    }

    [Fact]
    public async Task StoreAndFetchShouldWork()
    {
        var objectName = "my-sample";
        var content = "my-sample-content";
        using var contentMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        await BucketClient.Store(objectName, contentMemoryStream);

        var fetchedContent = await BucketClient.FetchAsBase64(objectName);
        Encoding.UTF8.GetString(Convert.FromBase64String(fetchedContent)).Should().Be(content);
    }
}
