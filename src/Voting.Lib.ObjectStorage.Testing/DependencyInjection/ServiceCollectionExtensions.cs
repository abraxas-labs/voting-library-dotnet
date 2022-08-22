// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.ObjectStorage;
using Voting.Lib.ObjectStorage.Testing.Mocks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// The service collection extensions for the object storage mocks.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Replaces the <see cref="ObjectStorageClient"/> with a mock implementation.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddVotingLibObjectStorageMock(this IServiceCollection services)
        => services
            .RemoveAll<IObjectStorageClient>()
            .AddSingleton<ObjectStorageClientMock>()
            .AddSingleton<IObjectStorageClient>(sp => sp.GetRequiredService<ObjectStorageClientMock>());
}
