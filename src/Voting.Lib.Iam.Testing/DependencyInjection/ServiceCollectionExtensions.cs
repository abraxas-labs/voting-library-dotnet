// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Iam.Services;
using Voting.Lib.Iam.Testing.Mocks;
using Voting.Lib.Iam.Testing.TokenHandling.OnBehalfToken;
using Voting.Lib.Iam.Testing.TokenHandling.ServiceToken;
using Voting.Lib.Iam.TokenHandling;
using Voting.Lib.Iam.TokenHandling.OnBehalfToken;
using Voting.Lib.Iam.TokenHandling.ServiceToken;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for IAM testing.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// replaces services with mock instances.
    /// </summary>
    /// <param name="services">the service collection.</param>
    /// <returns>the updated service collection.</returns>
    public static IServiceCollection AddVotingLibIamMocks(this IServiceCollection services)
        => services
            .RemoveAll<IServiceTokenHandlerFactory>()
            .AddSingleton<IServiceTokenHandlerFactory, ServiceTokenHandlerFactoryMock>()
            .RemoveAll<ITokenHandler>()
            .AddSingleton<ITokenHandler, ServiceTokenHandlerMock>()
            .RemoveAll<IOnBehalfTokenHandlerFactory>()
            .AddSingleton<IOnBehalfTokenHandlerFactory, OnBehalfTokenHandlerFactoryMock>()
            .RemoveAll<ITokenHandler>()
            .AddSingleton<ITokenHandler, OnBehalfTokenHandlerMock>()
            .RemoveAll<ITenantService>()
            .AddSingleton<ITenantService, TenantServiceMock>()
            .RemoveAll<IUserService>()
            .AddSingleton<IUserService, UserServiceMock>();
}
