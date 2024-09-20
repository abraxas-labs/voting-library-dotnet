// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.Grpc.Configuration;

/// <summary>
/// Grpc communication channel modes.
/// </summary>
public enum GrpcMode
{
    /// <summary>
    /// gRPC.
    /// </summary>
    Grpc,

    /// <summary>
    /// gRPC Web.
    /// </summary>
    GrpcWeb,

    /// <summary>
    /// gRPC Web Text.
    /// </summary>
    GrpcWebText,
}
