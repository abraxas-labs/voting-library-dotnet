// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Voting.Lib.UserNotifications;

/// <summary>
/// The smtp user notification sender.
/// </summary>
public class SmtpUserNotificationSender : IUserNotificationSender
{
    private readonly SmtpConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="SmtpUserNotificationSender"/> class.
    /// </summary>
    /// <param name="config">The config to use.</param>
    public SmtpUserNotificationSender(SmtpConfig config)
    {
        _config = config;
    }

    /// <inheritdoc />
    public async Task Send(UserNotification notification, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient(_config.Host, _config.Port);
        client.EnableSsl = _config.EnableSsl;
        if (_config.Username != null)
        {
            client.Credentials = new NetworkCredential(_config.Username, _config.Password);
        }

        var mailMessage = new MailMessage();
        mailMessage.To.Add(notification.RecipientEmail);
        mailMessage.From = new MailAddress(_config.FromAddress);
        mailMessage.Subject = notification.Subject;
        mailMessage.Body = notification.HtmlBody;
        mailMessage.IsBodyHtml = true;

        await client.SendMailAsync(mailMessage, cancellationToken);
    }
}
