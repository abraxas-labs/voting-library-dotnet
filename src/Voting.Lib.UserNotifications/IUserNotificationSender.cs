// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.UserNotifications;

/// <summary>
/// Sends user notifications.
/// </summary>
public interface IUserNotificationSender
{
    /// <summary>
    /// Sends the notification.
    /// </summary>
    /// <param name="notification">The notification.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    Task Send(UserNotification notification, CancellationToken cancellationToken);
}
