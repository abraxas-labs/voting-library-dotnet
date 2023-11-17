// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Voting.Lib.Testing.TestContainers;

namespace Voting.Lib.Messaging.IntegrationTest;

public static class RabbitMqTestContainer
{
    public const ushort ApiPort = 5672;
    public const ushort ManagementPort = 15672;
    public const string Username = "user";
    public const string Password = "password";
    private const string Image = "rabbitmq:3.8-management-alpine";

    public static IContainer Build()
    {
        return TestContainerBuilder.New("rabbitmq", Image)
            .WithEnvironment("RABBITMQ_DEFAULT_USER", Username)
            .WithEnvironment("RABBITMQ_DEFAULT_PASS", Password)
            .WithPortBinding(ApiPort, true)
            .WithPortBinding(ManagementPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(ApiPort))
            .Build();
    }
}
