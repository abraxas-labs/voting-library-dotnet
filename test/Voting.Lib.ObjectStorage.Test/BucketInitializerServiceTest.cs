// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Voting.Lib.ObjectStorage.Test;

public class BucketInitializerServiceTest
{
    [Fact]
    public async Task StartShouldEnsureBucketsExist()
    {
        var bucketClientMock = new Mock<IBucketObjectStorageClient>();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;

        bucketClientMock
            .Setup(x => x.EnsureBucketExists(cancellationToken))
            .Returns(Task.CompletedTask);

        var initializer = new BucketInitializerService(new[] { bucketClientMock.Object }, NullLogger<BucketInitializerService>.Instance);
        await initializer.StartAsync(cancellationToken);

        bucketClientMock.Verify(x => x.EnsureBucketExists(cancellationToken), Times.Once());
    }
}
