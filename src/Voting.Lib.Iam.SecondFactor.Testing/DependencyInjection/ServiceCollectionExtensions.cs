// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection.Extensions;
using Voting.Lib.Iam.SecondFactor.Services;
using Voting.Lib.Iam.SecondFactor.Testing.Mocks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service collection extensions for second factor mocks.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Replaces the second factor transaction service with a mock implementation.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddVotingLibIamSecondFactorMocks(this IServiceCollection services)
        => services
            .RemoveAll<ISecondFactorTransactionService>()
            .AddSingleton<SecondFactorTransactionServiceMock>()
            .AddSingleton<ISecondFactorTransactionService>(sp => sp.GetRequiredService<SecondFactorTransactionServiceMock>());
}
