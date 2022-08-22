// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DokConnector.Testing.Models;

/// <summary>
/// Mock data for the DOK connector.
/// </summary>
public class MockData
{
    internal MockData(string fileName, string messageType, byte[] data)
    {
        FileName = fileName;
        MessageType = messageType;
        Data = data;
    }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Gets the message type.
    /// </summary>
    public string MessageType { get; }

    /// <summary>
    /// Gets the data.
    /// </summary>
    public byte[] Data { get; }
}
