// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Iam.Models;

/// <summary>
/// Represents a second factor.
/// </summary>
public class SecondFactor
{
    internal SecondFactor(string code, string qr, ICollection<string> tokenJwtIds)
    {
        Code = code;
        Qr = qr;
        TokenJwtIds = tokenJwtIds;
    }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the qr code.
    /// </summary>
    public string Qr { get; set; }

    /// <summary>
    /// Gets or sets the token jwt ids.
    /// </summary>
    public ICollection<string> TokenJwtIds { get; set; }
}