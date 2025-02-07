// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Testcontainers.RabbitMq;

namespace Voting.Lib.Messaging.IntegrationTest;

public static class RabbitMqTestContainer
{
    public const ushort ApiPort = 5672;
    public const string Username = "user";
    public const string Password = "password";
    private const string Image = "rabbitmq:3.8-management-alpine";

    public static async Task<RabbitMqContainer> StartNew()
    {
        var container = new RabbitMqBuilder()
            .WithImage(Image)
            .WithUsername(Username)
            .WithPassword(Password)
            .Build();
        await container.StartAsync();
        return container;
    }
}
