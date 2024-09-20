// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Voting.Lib.Grpc.Interceptors;
using Xunit;

namespace Voting.Lib.Grpc.Test;

public class ExceptionInterceptorTest
{
    [Fact]
    public async Task NoDetailedErrorsNoExposedExceptionShouldOnlyMapStatusCode()
    {
        var ex = await InvokeWithException<TestException>(new TestExceptionInterceptor(false));
        ex.Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
        ex.Status.Detail.Should().Be(nameof(Exception));
    }

    [Fact]
    public async Task NoDetailedErrorsExposedExceptionShouldMapStatusCodeAndExceptionType()
    {
        var ex = await InvokeWithException<ExposedTestException>(new TestExceptionInterceptor(false));
        ex.Status.StatusCode.Should().Be(StatusCode.InvalidArgument);
        ex.Status.Detail.Should().Be(nameof(ExposedTestException));
    }

    [Fact]
    public async Task NoDetailedErrorsExposedExceptionMessageShouldMapStatusCodeAndExceptionMessage()
    {
        var ex = await InvokeWithException<ExposedMessageTestException>(new TestExceptionInterceptor(false));
        ex.Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
        ex.Status.Detail.Should().Be("Exception: exposed test message");
    }

    [Fact]
    public async Task DetailedErrorsShouldExposeDetails()
    {
        var ex = await InvokeWithException<TestException>(new TestExceptionInterceptor(true));
        ex.Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
        ex.Status.Detail.Should().Be("TestException: test message");
    }

    [Fact]
    public async Task ServerStreamingDetailedErrorsShouldExposeDetails()
    {
        var ex = await Assert.ThrowsAsync<RpcException>(async () => await new TestExceptionInterceptor(true).ServerStreamingServerHandler<object, object>(
            new(),
            new Mock<IServerStreamWriter<object>>().Object,
            new Mock<ServerCallContext>().Object,
            (_, _, _) => throw new TestException()));
        ex.Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
        ex.Status.Detail.Should().Be("TestException: test message");
    }

    [Fact]
    public async Task ClientStreamingDetailedErrorsShouldExposeDetails()
    {
        var ex = await Assert.ThrowsAsync<RpcException>(async () => await new TestExceptionInterceptor(true).ClientStreamingServerHandler<object, object>(
            new Mock<IAsyncStreamReader<object>>().Object,
            new Mock<ServerCallContext>().Object,
            (_, _) => throw new TestException()));
        ex.Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
        ex.Status.Detail.Should().Be("TestException: test message");
    }

    [Fact]
    public async Task DuplexStreamingDetailedErrorsShouldExposeDetails()
    {
        var ex = await Assert.ThrowsAsync<RpcException>(async () => await new TestExceptionInterceptor(true).DuplexStreamingServerHandler(
            new Mock<IAsyncStreamReader<object>>().Object,
            new Mock<IServerStreamWriter<object>>().Object,
            new Mock<ServerCallContext>().Object,
            (_, _, _) => throw new TestException()));
        ex.Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
        ex.Status.Detail.Should().Be("TestException: test message");
    }

    [Fact]
    public async Task DefaultShouldNotExposeDetails()
    {
        var ex = await InvokeWithException<ExposedTestException>(new SimpleExceptionInterceptor());
        ex.Status.StatusCode.Should().Be(StatusCode.Internal);
        ex.Status.Detail.Should().Be(nameof(Exception));
    }

    private async Task<RpcException> InvokeWithException<TException>(Interceptor interceptor)
        where TException : Exception, new()
    {
        return await Assert.ThrowsAsync<RpcException>(async () => await interceptor.UnaryServerHandler<object, object>(
            new(),
            new Mock<ServerCallContext>().Object,
            (_, _) => throw new TException()));
    }

    private class TestException : Exception
    {
        public TestException()
            : base("test message")
        {
        }
    }

    private class ExposedTestException : Exception
    {
        public ExposedTestException()
            : base("exposed test message")
        {
        }
    }

    private class ExposedMessageTestException : Exception
    {
        public ExposedMessageTestException()
            : base("exposed test message")
        {
        }
    }

    private class SimpleExceptionInterceptor : ExceptionInterceptor
    {
        public SimpleExceptionInterceptor()
            : base(NullLogger<ExceptionInterceptor>.Instance)
        {
        }

        protected override StatusCode MapExceptionToStatusCode(Exception ex)
            => StatusCode.Internal;
    }

    private class TestExceptionInterceptor : ExceptionInterceptor
    {
        public TestExceptionInterceptor(bool enableDetailedErrors)
            : base(NullLogger<ExceptionInterceptor>.Instance, enableDetailedErrors)
        {
        }

        protected override bool ExposeExceptionType(Exception ex)
            => ex is ExposedTestException;

        protected override bool ExposeExceptionMessage(Exception ex)
            => ex is ExposedMessageTestException;

        protected override StatusCode MapExceptionToStatusCode(Exception ex)
        {
            return ex switch
            {
                TestException => StatusCode.AlreadyExists,
                ExposedTestException => StatusCode.InvalidArgument,
                ExposedMessageTestException => StatusCode.AlreadyExists,
                _ => StatusCode.Internal,
            };
        }
    }
}
