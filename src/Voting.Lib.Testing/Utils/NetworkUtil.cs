// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net;
using System.Net.Sockets;

namespace Voting.Lib.Testing.Utils;

/// <summary>
/// A helper class for network related things.
/// </summary>
public static class NetworkUtil
{
    /// <summary>
    /// Returns a random free port.
    /// </summary>
    /// <returns>The number of the port.</returns>
    public static int FindFreePort()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Any, 0));
        return ((IPEndPoint)socket.LocalEndPoint!).Port;
    }
}
