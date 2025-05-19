// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using Microsoft.Extensions.DependencyInjection;

namespace Voting.Lib.UserNotifications;

/// <summary>
/// Extension methods to add user notification senders.
/// </summary>
public static class UserNotificationServiceCollectionExtensions
{
    /// <summary>
    /// Adds the smtp user notification sender.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The smtp config.</param>
    /// <returns>The service collection instance.</returns>
    public static IServiceCollection AddUserNotificationsSmtpSender(this IServiceCollection services, SmtpConfig config)
    {
        var senderFactory = ActivatorUtilities.CreateFactory<SmtpUserNotificationSender>([typeof(SmtpConfig)]);
        return services.AddScoped<IUserNotificationSender>(sp => senderFactory(sp, [config]));
    }
}
