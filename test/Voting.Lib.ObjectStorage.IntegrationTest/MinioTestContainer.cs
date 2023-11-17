// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Voting.Lib.Testing.TestContainers;

namespace Voting.Lib.ObjectStorage.IntegrationTest;

public static class MinioTestContainer
{
    public const ushort ApiPort = 9000;
    public const ushort ConsolePort = 9001;
    public const string Username = "user";
    public const string Password = "password";
    private const string Image = "minio/minio:RELEASE.2022-05-08T23-50-31Z";

    public static IContainer Build()
    {
        return TestContainerBuilder.New("minio", Image)
            .WithEnvironment("MINIO_ROOT_USER", Username)
            .WithEnvironment("MINIO_ROOT_PASSWORD", Password)
            .WithPortBinding(ApiPort, true)
            .WithPortBinding(ConsolePort, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCurlHttpOk(ApiPort, "/minio/health/live"))
            .WithCommand("server", "/data", "--console-address", ":" + ConsolePort)
            .Build();
    }
}
