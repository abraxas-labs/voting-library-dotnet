// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Voting.Lib.Testing.TestContainers;

/// <summary>
/// Helper methods to build a new test container.
/// </summary>
public static class TestContainerBuilder
{
    /// <summary>
    /// Creates a new <see cref="ITestcontainersBuilder{TDockerContainer}"/> initialized with voting default values.
    /// </summary>
    /// <param name="serviceName">The name of the service.</param>
    /// <param name="image">The docker image.</param>
    /// <returns>The created builder.</returns>
    public static ITestcontainersBuilder<TestcontainersContainer> New(string serviceName, string image)
    {
        var builder = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage(image)
            .WithName($"voting-test-{serviceName}-{Guid.NewGuid()}");

        if (Environment.GetEnvironmentVariable("DOCKER_HOST") is { } dockerHost)
        {
            builder = builder.WithDockerEndpoint(dockerHost);
        }

        return builder;
    }
}
