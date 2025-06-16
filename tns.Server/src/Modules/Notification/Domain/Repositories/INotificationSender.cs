using tns.Server.src.Modules.Notification.Domain.Entities;

namespace tns.Server.src.Modules.Notification.Domain.Repositories
{
    public interface INotificationSender
    {
        Task SendEmailAsync(EmailMessage message, SmtpConfiguration configuration, CancellationToken cancellationToken);

    }
}
