// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using Voting.Lib.ObjectStorage.Config;

namespace Voting.Lib.ObjectStorage.IntegrationTest;

public class SampleBucketClient : BucketObjectStorageClient
{
    public SampleBucketClient(IObjectStorageClient client, ObjectStorageBucketConfig config)
        : base(config, client)
    {
    }
}
