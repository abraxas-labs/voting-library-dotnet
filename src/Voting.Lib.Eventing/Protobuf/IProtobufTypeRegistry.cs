// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using Google.Protobuf.Reflection;

namespace Voting.Lib.Eventing.Protobuf;

/// <summary>
/// Protobuf type registry to resolve protobuf message descriptors.
/// </summary>
public interface IProtobufTypeRegistry
{
    /// <summary>
    /// Attempts to find a message descriptor by its full name.
    /// </summary>
    /// <param name="fullName">The full name of the message, which is the dot-separated
    /// combination of package, containing messages and message name.</param>
    /// <returns>The message descriptor corresponding to <paramref name="fullName"/> or null
    /// if there is no such message descriptor.</returns>
    MessageDescriptor? Find(string fullName);

    /// <summary>
    /// Attempts to find a message descriptor by its CLR type.
    /// </summary>
    /// <param name="messageType">The CLR type of the message.</param>
    /// <returns>The found message descriptor or <c>null</c> if there is no matching message descriptor.</returns>
    MessageDescriptor? Find(Type messageType);
}
