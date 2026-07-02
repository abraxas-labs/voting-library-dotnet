// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Voting.Lib.Common;
using Voting.Lib.Iam.Models;
using Voting.Lib.Iam.SecondFactor.Configuration;
using Voting.Lib.Iam.SecondFactor.Exceptions;
using Voting.Lib.Iam.SecondFactor.Models;
using Voting.Lib.Iam.SecondFactor.Services;
using Voting.Lib.Iam.Services;
using Voting.Lib.Iam.Services.ApiClient.Identity;
using Voting.Lib.Iam.Store;
using Voting.Lib.Testing.Mocks;
using Xunit;
using SecondFactorModel = Voting.Lib.Iam.Models.SecondFactor;

namespace Voting.Lib.Iam.SecondFactor.Test;

public class SecondFactorTransactionServiceTest
{
    private const string LoginId = "user-1";

    [Fact]
    public async Task CreateShouldPersistTransactionAndFilterSupportedProviders()
    {
        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(x => x.GetReadySecondFactorProviders(LoginId))
            .ReturnsAsync(new HashSet<V1SecondFactorProvider>
            {
                V1SecondFactorProvider.OTP,
                V1SecondFactorProvider.SMS,
            });

        var repoMock = new Mock<ISecondFactorTransactionRepository>();
        SecondFactorTransaction? createdTransaction = null;
        repoMock
            .Setup(x => x.Create(It.IsAny<SecondFactorTransaction>()))
            .Callback<SecondFactorTransaction>(transaction => createdTransaction = transaction)
            .Returns(Task.CompletedTask);

        var config = new SecondFactorTransactionConfig
        {
            TransactionExpiration = TimeSpan.FromMinutes(15),
            CorrelationCodeLength = 4,
        };
        var service = BuildService(userServiceMock.Object, repoMock.Object, config);

        var result = await service.Create(new TestActionId("action-hash"), "Approve publication");

        result.AvailableProviders.Should().BeEquivalentTo([V1SecondFactorProvider.OTP]);
        result.Nevis.Should().BeNull();
        result.CorrelationCode.Should().MatchRegex("^[ABCDEFGHJKLMNPQRSTUVWXYZ23456789]{4}$");
        result.Message.Should().Be($"({result.CorrelationCode}) Approve publication");

        createdTransaction.Should().NotBeNull();
        createdTransaction!.UserId.Should().Be(LoginId);
        createdTransaction.ActionIdHash.Should().Be("action-hash");
        createdTransaction.LastUpdatedAt.Should().Be(MockedClock.UtcNowDate);
        createdTransaction.ExpireAt.Should().Be(MockedClock.UtcNowDate.Add(config.TransactionExpiration));
        createdTransaction.NevisExternalTokenJwtIds.Should().BeNull();

        userServiceMock.Verify(x => x.RequestSecondFactor(It.IsAny<string>(), It.IsAny<V1SecondFactorProvider>(), It.IsAny<string?>()), Times.Never);
        repoMock.Verify(x => x.Create(It.IsAny<SecondFactorTransaction>()), Times.Once);
    }

    [Fact]
    public async Task CreateShouldRequestNevisWhenAvailable()
    {
        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(x => x.GetReadySecondFactorProviders(LoginId))
            .ReturnsAsync(new HashSet<V1SecondFactorProvider>
            {
                V1SecondFactorProvider.OTP,
                V1SecondFactorProvider.NEVIS,
                V1SecondFactorProvider.SMS,
            });

        string? requestedMessage = null;
        userServiceMock
            .Setup(x => x.RequestSecondFactor(LoginId, V1SecondFactorProvider.NEVIS, It.IsAny<string?>()))
            .Callback<string, V1SecondFactorProvider, string?>((_, _, message) => requestedMessage = message)
            .ReturnsAsync(CreateSecondFactor(
                V1SecondFactorProvider.NEVIS,
                CreateSecondFactorNevisInfo("qr-code", "jwt-1", "jwt-2")));

        var repoMock = new Mock<ISecondFactorTransactionRepository>();
        SecondFactorTransaction? createdTransaction = null;
        repoMock
            .Setup(x => x.Create(It.IsAny<SecondFactorTransaction>()))
            .Callback<SecondFactorTransaction>(transaction => createdTransaction = transaction)
            .Returns(Task.CompletedTask);

        var service = BuildService(userServiceMock.Object, repoMock.Object, new SecondFactorTransactionConfig());

        var result = await service.Create(new TestActionId("action-hash"), "Approve publication");

        result.AvailableProviders.Should().BeEquivalentTo([V1SecondFactorProvider.OTP, V1SecondFactorProvider.NEVIS]);
        result.Nevis.Should().NotBeNull();
        result.Nevis!.QrCode.Should().Be("qr-code");
        result.Nevis.TokenJwtIds.Should().Equal("jwt-1", "jwt-2");
        result.Message.Should().Be(requestedMessage);

        createdTransaction.Should().NotBeNull();
        createdTransaction!.NevisExternalTokenJwtIds.Should().Equal("jwt-1", "jwt-2");

        userServiceMock.Verify(x => x.RequestSecondFactor(LoginId, V1SecondFactorProvider.NEVIS, It.Is<string?>(message => message == requestedMessage)), Times.Once);
    }

    [Fact]
    public async Task EnsureVerifiedShouldThrowWhenVerificationFails()
    {
        var transactionId = Guid.NewGuid();
        var transaction = new SecondFactorTransaction
        {
            Id = transactionId,
            ActionIdHash = "action-hash",
            NevisExternalTokenJwtIds = ["jwt-1"],
        };

        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(x => x.VerifySecondFactor(LoginId, V1SecondFactorProvider.OTP, "123456", transaction.NevisExternalTokenJwtIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var repoMock = new Mock<ISecondFactorTransactionRepository>();
        repoMock
            .Setup(x => x.GetById(transactionId))
            .ReturnsAsync(transaction);
        repoMock
            .Setup(x => x.Update(transaction))
            .Returns(Task.CompletedTask);

        var service = BuildService(userServiceMock.Object, repoMock.Object, new SecondFactorTransactionConfig());
        var actionProviderCalled = false;

        await service
            .Invoking(s => s.EnsureVerified(
                transactionId,
                V1SecondFactorProvider.OTP,
                () =>
                {
                    actionProviderCalled = true;
                    return Task.FromResult<IActionId>(new TestActionId("action-hash"));
                },
                "123456"))
            .Should()
            .ThrowAsync<SecondFactorTransactionNotVerifiedException>();

        actionProviderCalled.Should().BeFalse();
        transaction.PollCount.Should().Be(1);
        transaction.LastAttemptedProvider.Should().Be(V1SecondFactorProvider.OTP);
        transaction.LastUpdatedAt.Should().Be(MockedClock.UtcNowDate);

        repoMock.Verify(x => x.GetById(transactionId), Times.Once);
        repoMock.Verify(x => x.Update(transaction), Times.Once);
    }

    [Fact]
    public async Task EnsureVerifiedShouldThrowWhenActionHashChanged()
    {
        var transactionId = Guid.NewGuid();
        var transaction = new SecondFactorTransaction
        {
            Id = transactionId,
            ActionIdHash = "original-action-hash",
        };

        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(x => x.VerifySecondFactor(LoginId, V1SecondFactorProvider.OTP, null, transaction.NevisExternalTokenJwtIds, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var repoMock = new Mock<ISecondFactorTransactionRepository>();
        repoMock
            .SetupSequence(x => x.GetById(transactionId))
            .ReturnsAsync(transaction)
            .ReturnsAsync(transaction);
        repoMock
            .Setup(x => x.Update(transaction))
            .Returns(Task.CompletedTask);

        var service = BuildService(userServiceMock.Object, repoMock.Object, new SecondFactorTransactionConfig());

        await service
            .Invoking(s => s.EnsureVerified(
                transactionId,
                V1SecondFactorProvider.OTP,
                () => Task.FromResult<IActionId>(new TestActionId("changed-action-hash"))))
            .Should()
            .ThrowAsync<SecondFactorTransactionDataChangedException>();

        transaction.PollCount.Should().Be(1);
        transaction.LastAttemptedProvider.Should().Be(V1SecondFactorProvider.OTP);
        transaction.LastUpdatedAt.Should().Be(MockedClock.UtcNowDate);

        repoMock.Verify(x => x.GetById(transactionId), Times.Exactly(2));
        repoMock.Verify(x => x.Update(transaction), Times.Once);
    }

    private static SecondFactorTransactionService BuildService(
        IUserService userService,
        ISecondFactorTransactionRepository repo,
        SecondFactorTransactionConfig config)
    {
        var authMock = new Mock<IAuth>();
        authMock
            .Setup(x => x.User)
            .Returns(new User { Loginid = LoginId });

        return new SecondFactorTransactionService(
            authMock.Object,
            userService,
            config,
            NullLogger<SecondFactorTransactionService>.Instance,
            repo,
            MockedClock.CreateFakeTimeProvider());
    }

    private static SecondFactorModel CreateSecondFactor(V1SecondFactorProvider provider, SecondFactorNevisInfo? nevis = null)
        => new(provider, nevis);

    private static SecondFactorNevisInfo CreateSecondFactorNevisInfo(string qrCode, params string[] tokenJwtIds)
        => new(qrCode, tokenJwtIds);

    private sealed class TestActionId(string hash) : IActionId
    {
        public string ComputeHash() => hash;
    }
}
