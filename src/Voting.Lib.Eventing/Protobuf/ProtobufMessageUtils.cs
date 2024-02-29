// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Voting.Lib.Eventing.Protobuf;

internal static class ProtobufMessageUtils
{
    private const string DescriptorField = "Descriptor";

    public static IEnumerable<MessageDescriptor> MessageDescriptorsOfAssemblies(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IMessage).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
            .Select(GetDescriptor)
            .Where(md => md != null)
            .Select(x => x!);
    }

    private static MessageDescriptor? GetDescriptor(Type t)
    {
        var pi = t.GetProperty(DescriptorField, BindingFlags.Public | BindingFlags.Static);
        return pi?.GetValue(null) as MessageDescriptor;
    }
}
