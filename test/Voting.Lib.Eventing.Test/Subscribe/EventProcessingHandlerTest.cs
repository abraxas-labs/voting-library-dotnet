// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using FluentAssertions;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Voting.Lib.Eventing.Protobuf;
using Voting.Lib.Eventing.Subscribe;
using Voting.Lib.Eventing.Test.Events;
using Xunit;

namespace Voting.Lib.Eventing.Test.Subscribe;

public class EventProcessingHandlerTest
{
    [Fact]
    public async Task HandleEventShouldProcessAndUpdateScope()
    {
        var subscription = new Subscription<TransientEventProcessorScope>(
            new[] { typeof(TestEvent) },
            WellKnownStreams.CategoryVoting);

        var protoRegistry = ProtobufTypeRegistry.CreateByScanningAssemblies(new[] { typeof(TestEvent).Assembly });
        var adapterRegistry = new EventProcessorAdapterRegistry<TransientEventProcessorScope>(protoRegistry);

        var handlerMock = new Mock<ICatchUpDetectorEventProcessor<TransientEventProcessorScope, TestEvent>>();
        handlerMock
            .Setup(x => x.Process(It.IsAny<TestEvent>(), It.IsAny<bool>()))
            .Returns(Task.CompletedTask);

        var scope = new TransientEventProcessorScope();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(new EventProcessorAdapter<TransientEventProcessorScope, TestEvent>(handlerMock.Object, new ProtobufJsonSerializer(JsonFormatter.Default, JsonParser.Default, protoRegistry)))
            .AddSingleton(scope)
            .BuildServiceProvider(true);

        var handler = new EventProcessingHandler<TransientEventProcessorScope>(
            adapterRegistry,
            NullLogger<EventProcessingHandler<TransientEventProcessorScope>>.Instance,
            serviceProvider.GetRequiredService<IServiceScopeFactory>());

        var handled = await handler.HandleEvent(subscription, new(EventRecordTestUtil.BuildRecord(), null, null));
        handled.Should().BeTrue();

        var position = await scope.GetSnapshotPosition();
        position!.Value.Item2.Should().Be(10UL);
    }

    [Fact]
    public async Task HandleEventShouldNotUpdateScopeIfNoHandler()
    {
        var subscription = new Subscription<TransientEventProcessorScope>(
            new[] { typeof(TestEvent) },
            WellKnownStreams.CategoryVoting);

        var protoRegistry = ProtobufTypeRegistry.CreateByScanningAssemblies(new[] { typeof(TestEvent).Assembly });
        var adapterRegistry = new EventProcessorAdapterRegistry<TransientEventProcessorScope>(protoRegistry);

        var scope = new TransientEventProcessorScope();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(scope)
            .BuildServiceProvider(true);

        var handler = new EventProcessingHandler<TransientEventProcessorScope>(
            adapterRegistry,
            NullLogger<EventProcessingHandler<TransientEventProcessorScope>>.Instance,
            serviceProvider.GetRequiredService<IServiceScopeFactory>());

        var handled = await handler.HandleEvent(subscription, new(EventRecordTestUtil.BuildRecord(), null, null));
        handled.Should().BeFalse();

        var position = await scope.GetSnapshotPosition();
        position.Should().BeNull();
    }

    [Fact]
    public async Task HandleCatchUpCompletedShouldCallCompleters()
    {
        var subscription = new Subscription<TransientEventProcessorScope>(
            new[] { typeof(TestEvent) },
            WellKnownStreams.CategoryVoting);

        var protoRegistry = ProtobufTypeRegistry.CreateByScanningAssemblies(new[] { typeof(TestEvent).Assembly });
        var adapterRegistry = new EventProcessorAdapterRegistry<TransientEventProcessorScope>(protoRegistry);

        var completerMock = new Mock<IEventProcessorCatchUpCompleter<TransientEventProcessorScope>>();
        completerMock
            .Setup(x => x.CatchUpCompleted())
            .Returns(Task.CompletedTask);

        var serviceProvider = new ServiceCollection()
            .AddSingleton(completerMock.Object)
            .BuildServiceProvider(true);

        var handler = new EventProcessingHandler<TransientEventProcessorScope>(
            adapterRegistry,
            NullLogger<EventProcessingHandler<TransientEventProcessorScope>>.Instance,
            serviceProvider.GetRequiredService<IServiceScopeFactory>());

        await handler.HandleCatchUpCompleted(subscription);
        completerMock.Verify(x => x.CatchUpCompleted(), Times.Once());
    }
}
