// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.Iam.Models;

/// <summary>
/// Represents a nevis second factor.
/// </summary>
public class SecondFactorNevisInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecondFactorNevisInfo"/> class.
    /// </summary>
    /// <param name="qrCode">The qr code.</param>
    /// <param name="tokenJwtIds">The jwt ids.</param>
    public SecondFactorNevisInfo(
        string qrCode,
        ICollection<string> tokenJwtIds)
    {
        QrCode = qrCode;
        TokenJwtIds = tokenJwtIds;
    }

    /// <summary>
    /// Gets or sets the qr code.
    /// </summary>
    public string QrCode { get; set; }

    /// <summary>
    /// Gets or sets the token jwt ids.
    /// </summary>
    public ICollection<string> TokenJwtIds { get; set; }
}
