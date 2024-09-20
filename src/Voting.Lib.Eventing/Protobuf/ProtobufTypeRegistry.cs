// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf.Reflection;

namespace Voting.Lib.Eventing.Protobuf;

internal sealed class ProtobufTypeRegistry : IProtobufTypeRegistry
{
    private readonly IReadOnlyDictionary<string, MessageDescriptor> _fullNameToMessageMap;
    private readonly IReadOnlyDictionary<Type, MessageDescriptor> _clrTypeToMessageMap;

    private ProtobufTypeRegistry(IEnumerable<MessageDescriptor> descriptors)
    {
        // instead of the protobuf default TypeRegistry, we use our own implementation
        // to handle files with the same name correctly and to find the descriptor case-insensitive.
        _fullNameToMessageMap = descriptors.ToDictionary(x => x.FullName, StringComparer.InvariantCultureIgnoreCase);
        _clrTypeToMessageMap = descriptors.ToDictionary(x => x.ClrType);
    }

    /// <inheritdoc />
    public MessageDescriptor? Find(string fullName)
        => _fullNameToMessageMap.GetValueOrDefault(fullName);

    /// <inheritdoc />
    public MessageDescriptor? Find(Type messageType)
        => _clrTypeToMessageMap.GetValueOrDefault(messageType);

    internal static ProtobufTypeRegistry CreateByScanningAssemblies(IEnumerable<Assembly> assemblies)
    {
        var descriptors = ProtobufMessageUtils.MessageDescriptorsOfAssemblies(assemblies);
        return new ProtobufTypeRegistry(descriptors);
    }
}
