// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Voting.Lib.Eventing.Persistence;

/// <summary>
/// Provides metadata message descriptor for a given event.
/// </summary>
public interface IMetadataDescriptorProvider
{
    /// <summary>
    /// Provides the metadata descriptor for a given event.
    /// </summary>
    /// <param name="eventMessage">The event message.</param>
    /// <returns>The descriptor of the metadata type of the given event.</returns>
    MessageDescriptor? GetMetadataDescriptor(IMessage eventMessage);
}
