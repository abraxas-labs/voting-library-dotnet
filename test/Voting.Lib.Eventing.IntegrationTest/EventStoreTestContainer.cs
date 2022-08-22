// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Runtime.InteropServices;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Voting.Lib.Testing.TestContainers;

namespace Voting.Lib.Eventing.IntegrationTest;

public static class EventStoreTestContainer
{
    public const ushort HttpPort = 2113;
    private const string Image = "eventstore/eventstore:21.10.0-bionic";
    private const string ArmImage = "ghcr.io/eventstore/eventstore:21.10.1-alpha-arm64v8";

    public static TestcontainersContainer Build()
    {
        // EventStore does not yet support ARM via multi arch builds, only as separate image
        var image = RuntimeInformation.ProcessArchitecture == Architecture.Arm64
            ? ArmImage
            : Image;

        return TestContainerBuilder.New("event-store", image)
            .WithEnvironment("EVENTSTORE_MEM_DB", "true")
            .WithEnvironment("EVENTSTORE_CLUSTER_SIZE", "1")
            .WithEnvironment("EVENTSTORE_EXT_IP", "0.0.0.0")
            .WithEnvironment("EVENTSTORE_ENABLE_ATOM_PUB_OVER_HTTP", "true")
            .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS", "SYSTEM")
            .WithEnvironment("EVENTSTORE_START_STANDARD_PROJECTIONS", "true")
            .WithEnvironment("EVENTSTORE_INSECURE", "true")
            .WithPortBinding(HttpPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCurlHttpOk(HttpPort, "/health/live"))
            .Build();
    }
}
