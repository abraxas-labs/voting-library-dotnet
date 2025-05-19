// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Google.Protobuf;
using Google.Protobuf.Reflection;
using Voting.Lib.Eventing.Persistence;
using Voting.Lib.Eventing.Test.Events;

namespace Voting.Lib.Eventing.Test;

public class TestMetadataProvider : IMetadataDescriptorProvider
{
    public MessageDescriptor? GetMetadataDescriptor(IMessage eventMessage) => TestEventMetadata.Descriptor;
}
