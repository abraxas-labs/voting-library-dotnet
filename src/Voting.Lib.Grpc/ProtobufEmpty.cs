// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace Voting.Lib.Grpc;

/// <summary>
/// Static instances of the <see cref="Empty"/> protobuf message.
/// Can be used to reduce GC pressure.
/// </summary>
public static class ProtobufEmpty
{
    /// <summary>
    /// Gets the <see cref="Empty"/> instance.
    /// </summary>
    public static readonly Empty Instance = new();

    /// <summary>
    /// Gets the <see cref="Empty"/> instance as a task.
    /// </summary>
    public static readonly Task<Empty> InstanceTask = Task.FromResult(Instance);
}
