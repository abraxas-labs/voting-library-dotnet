// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using FluentAssertions;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Voting.Lib.Eventing.Persistence;
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
            [typeof(TestEvent)],
            WellKnownStreams.CategoryVoting);

        var protoRegistry = ProtobufTypeRegistry.CreateByScanningAssemblies([typeof(TestEvent).Assembly]);
        var adapterRegistry = new EventProcessorAdapterRegistry<TransientEventProcessorScope>(protoRegistry);

        var scope = new TransientEventProcessorScope();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IEventSerializer>(new ProtobufJsonSerializer(JsonFormatter.Default, JsonParser.Default, protoRegistry))
            .AddScoped<EventProcessorAdapter<TransientEventProcessorScope, TestEvent>>()
            .AddScoped<ICatchUpDetectorEventProcessor<TransientEventProcessorScope, TestEvent>, TestEventProcessor>()
            .AddSingleton(scope)
            .AddScoped<EventProcessorContextAccessor>()
            .BuildServiceProvider(true);

        var handler = new EventProcessingHandler<TransientEventProcessorScope>(
            adapterRegistry,
            NullLogger<EventProcessingHandler<TransientEventProcessorScope>>.Instance,
            serviceProvider.GetRequiredService<IEventSerializer>(),
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
            [typeof(TestEvent)],
            WellKnownStreams.CategoryVoting);

        var protoRegistry = ProtobufTypeRegistry.CreateByScanningAssemblies([typeof(TestEvent).Assembly]);
        var adapterRegistry = new EventProcessorAdapterRegistry<TransientEventProcessorScope>(protoRegistry);

        var scope = new TransientEventProcessorScope();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(scope)
            .BuildServiceProvider(true);

        var handler = new EventProcessingHandler<TransientEventProcessorScope>(
            adapterRegistry,
            NullLogger<EventProcessingHandler<TransientEventProcessorScope>>.Instance,
            new ProtobufJsonSerializer(JsonFormatter.Default, JsonParser.Default, protoRegistry),
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
            [typeof(TestEvent)],
            WellKnownStreams.CategoryVoting);

        var protoRegistry = ProtobufTypeRegistry.CreateByScanningAssemblies([typeof(TestEvent).Assembly]);
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
            new ProtobufJsonSerializer(JsonFormatter.Default, JsonParser.Default, protoRegistry),
            serviceProvider.GetRequiredService<IServiceScopeFactory>());

        await handler.HandleCatchUpCompleted(subscription);
        completerMock.Verify(x => x.CatchUpCompleted(), Times.Once());
    }

    private class TestEventProcessor : ICatchUpDetectorEventProcessor<TransientEventProcessorScope, TestEvent>
    {
        private readonly EventProcessorContextAccessor _eventProcessorContextAccessor;

        public TestEventProcessor(EventProcessorContextAccessor eventProcessorContextAccessor)
        {
            _eventProcessorContextAccessor = eventProcessorContextAccessor;
        }

        public Task Process(TestEvent eventData, bool isCatchUp)
        {
            _eventProcessorContextAccessor.Context.IsCatchUp.Should().Be(isCatchUp);
            return Task.CompletedTask;
        }
    }
}
