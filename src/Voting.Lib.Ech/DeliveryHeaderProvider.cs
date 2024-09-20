// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Ech0058_5_0;
using Voting.Lib.Common;
using Voting.Lib.Ech.Configuration;

namespace Voting.Lib.Ech;

/// <summary>
/// Provider for eCH delivery headers.
/// </summary>
public class DeliveryHeaderProvider
{
    private const ActionType ActionNew = ActionType.Item1; // new
    private const int MaxManufacturerLength = 50;
    private const int MaxProductLength = 30;
    private const int MaxProductVersionLength = 10;

    private readonly EchConfig _config;
    private readonly IClock _clock;
    private readonly IEchMessageIdProvider _messageIdProvider;
    private readonly SendingApplicationType _sendingApplication;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeliveryHeaderProvider"/> class.
    /// </summary>
    /// <param name="config">The eCH configuration.</param>
    /// <param name="clock">The clock.</param>
    /// <param name="messageIdProvider">The eCH message ID provider.</param>
    public DeliveryHeaderProvider(EchConfig config, IClock clock, IEchMessageIdProvider messageIdProvider)
    {
        _config = config;
        _clock = clock;
        _messageIdProvider = messageIdProvider;
        _sendingApplication = new SendingApplicationType
        {
            Manufacturer = Truncate(_config.Manufacturer, MaxManufacturerLength),
            Product = Truncate(_config.Product, MaxProductLength),
            ProductVersion = Truncate(_config.ProductVersion, MaxProductVersionLength),
        };
    }

    /// <summary>
    /// Builds a new delivery header.
    /// </summary>
    /// <param name="isTestingPhase">Whether testing phase is in progress (true) or not (false).</param>
    /// <returns>The created header.</returns>
    public HeaderType BuildHeader(bool isTestingPhase)
    {
        return BuildHeaderType(_config.TestDeliveryFlag || isTestingPhase);
    }

    /// <summary>
    /// Builds a new delivery header.
    /// </summary>
    /// <returns>The created header.</returns>
    public HeaderType BuildHeader()
    {
        return BuildHeaderType(_config.TestDeliveryFlag);
    }

    private static string Truncate(string s, int maxLength)
        => s.Length > maxLength ? s[..maxLength] : s;

    private HeaderType BuildHeaderType(bool testDeliveryFlag)
    {
        return new HeaderType
        {
            Action = ActionNew,
            MessageId = _messageIdProvider.NewId(),
            SendingApplication = _sendingApplication,
            TestDeliveryFlag = testDeliveryFlag,
            SenderId = _config.SenderId,
            MessageDate = _clock.UtcNow,
            MessageType = _config.MessageType,
        };
    }
}
