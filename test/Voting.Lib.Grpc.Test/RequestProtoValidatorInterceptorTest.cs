// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FluentAssertions;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Test.Messages;
using Voting.Lib.Grpc.Interceptors;
using Voting.Lib.ProtoValidation;
using Xunit;

namespace Voting.Lib.Grpc.Test;

public class RequestProtoValidatorInterceptorTest : IAsyncDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly RequestProtoValidatorInterceptor _interceptor;

    public RequestProtoValidatorInterceptorTest()
    {
        _serviceProvider = new ServiceCollection()
            .AddProtoValidators()
            .BuildServiceProvider();

        _interceptor = new RequestProtoValidatorInterceptor(_serviceProvider.GetRequiredService<ProtoValidator>());
    }

    [Fact]
    public async Task ValidationShouldSucceed()
    {
        var result = await CallUnaryInterceptor(new TestRequest { RequiredString = "message" });
        result.Should().Be("ok");
    }

    [Fact]
    public async Task ValidationServerStreamingShouldSucceed()
    {
        var exception = await Record.ExceptionAsync(() => CallServerStreamingInterceptor(new TestRequest { RequiredString = "message" }));
        Assert.Null(exception);
    }

    [Fact]
    public Task ValidationShouldThrowOnFailure()
    {
        return Assert.ThrowsAsync<ValidationException>(async () => await CallUnaryInterceptor(new TestRequest()));
    }

    [Fact]
    public Task ValidationServerStreamingShouldThrowOnFailure()
    {
        return Assert.ThrowsAsync<ValidationException>(async () => await CallServerStreamingInterceptor(new TestRequest()));
    }

    public async ValueTask DisposeAsync()
    {
        await _serviceProvider.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    private async Task<string> CallUnaryInterceptor<TRequest>(TRequest request)
        where TRequest : class
    {
        return await _interceptor.UnaryServerHandler(request, new Mock<ServerCallContext>().Object, (_, _) => Task.FromResult("ok"));
    }

    private async Task CallServerStreamingInterceptor<TRequest>(TRequest request)
        where TRequest : class
    {
        await _interceptor.ServerStreamingServerHandler(
            request,
            new Mock<IServerStreamWriter<object>>().Object,
            new Mock<ServerCallContext>().Object,
            (_, _, _) => Task.FromResult("ok"));
    }
}
