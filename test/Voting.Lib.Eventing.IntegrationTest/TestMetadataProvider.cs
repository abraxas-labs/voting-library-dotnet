// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Google.Protobuf;
using Google.Protobuf.Reflection;
using Voting.Lib.Eventing.IntegrationTest.Events;
using Voting.Lib.Eventing.Persistence;

namespace Voting.Lib.Eventing.IntegrationTest;

public class TestMetadataProvider : IMetadataDescriptorProvider
{
    public MessageDescriptor? GetMetadataDescriptor(IMessage eventMessage) => TestEventMetadata.Descriptor;
}
