// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Voting.Lib.Grpc.Interceptors;
using Xunit;

namespace Voting.Lib.Grpc.Test;

public class RequestValidatorInterceptorTest : IAsyncDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly RequestValidatorInterceptor _interceptor;
    private readonly TestRequestValidator _validator;

    public RequestValidatorInterceptorTest()
    {
        _serviceProvider = new ServiceCollection()
            .AddForwardRefSingleton<IValidator<TestRequest>, TestRequestValidator>()
            .BuildServiceProvider();

        _validator = _serviceProvider.GetRequiredService<TestRequestValidator>();
        _interceptor = new RequestValidatorInterceptor(_serviceProvider);
    }

    [Fact]
    public async Task ValidationShouldSucceed()
    {
        var result = await CallUnaryInterceptor(new TestRequest { IsValid = true });
        result.Should().Be("ok");
        _validator.ValidationCalled.Should().Be(1);
    }

    [Fact]
    public async Task ValidationServerStreamingShouldSucceed()
    {
        await CallServerStreamingInterceptor(new TestRequest { IsValid = true });
        _validator.ValidationCalled.Should().Be(1);
    }

    [Fact]
    public Task ValidationShouldThrowOnFailure()
    {
        return Assert.ThrowsAsync<ValidationException>(async () => await CallUnaryInterceptor(new TestRequest { IsValid = false }));
    }

    [Fact]
    public Task ValidationServerStreamingShouldThrowOnFailure()
    {
        return Assert.ThrowsAsync<ValidationException>(async () => await CallServerStreamingInterceptor(new TestRequest { IsValid = false }));
    }

    [Fact]
    public async Task ShouldSucceedIfNoValidatorIsAvailable()
    {
        var result = await CallUnaryInterceptor("test");
        result.Should().Be("ok");
        _validator.ValidationCalled.Should().Be(0);
    }

    [Fact]
    public async Task ServerStreamingShouldSucceedIfNoValidatorIsAvailable()
    {
        await CallServerStreamingInterceptor("test");
        _validator.ValidationCalled.Should().Be(0);
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

    private class TestRequest
    {
        public bool IsValid { get; set; }
    }

    private class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.IsValid).Equal(true);
        }

        public int ValidationCalled { get; set; }

        public override async Task<ValidationResult> ValidateAsync(
            ValidationContext<TestRequest> context,
            CancellationToken cancellation = default)
        {
            ValidationCalled++;
            return await base.ValidateAsync(context, cancellation);
        }
    }
}
