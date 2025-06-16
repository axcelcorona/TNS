using tns.Server.src.Modules.Notification.Domain.Repositories;
using tns.Server.src.Modules.Notification.Domain.Entities;
using tns.Server.src.Modules.Notification.Domain;
using System.Net.Mail;
using System.Net;

namespace tns.Server.src.Modules.Notification.Infrastructure.Smtp
{
    public class SmtpNotificationSender : INotificationSender
    {
        public async Task SendEmailAsync(EmailMessage message, SmtpConfiguration configuration, CancellationToken cancellationToken)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            using (var smtpClient = new SmtpClient(configuration.Host, configuration.Port))
            {
                smtpClient.Credentials = new NetworkCredential(configuration.Username, configuration.Password);
                smtpClient.EnableSsl = configuration.EnableSsl;

                var mailMessage = new MailMessage(message.From, message.To)
                {
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = true 
                };

                await smtpClient.SendMailAsync(mailMessage, cancellationToken);
            }
        }
    }
}
