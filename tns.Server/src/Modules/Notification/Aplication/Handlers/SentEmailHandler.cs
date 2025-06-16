using MediatR;
using Microsoft.Extensions.Options;
using tns.Server.src.Modules.Notification.Aplication.Commands;
using tns.Server.src.Modules.Notification.Domain;
using tns.Server.src.Modules.Notification.Domain.Entities;
using tns.Server.src.Modules.Notification.Domain.Repositories;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.Notification.Aplication.Handlers
{
    public class SentEmailHandler : IRequestHandler<SendEmailCommand, Result<bool>>
    {
        private readonly INotificationSender _smtpRepository;
        private readonly SmtpConfiguration _smtpConfiguration;
        public SentEmailHandler(INotificationSender smtpRepository, IOptions<SmtpConfiguration> options)
        {
            _smtpRepository = smtpRepository ?? throw new ArgumentNullException(nameof(smtpRepository));
            _smtpConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }
        public async Task<Result<bool>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.to))
                return Result<bool>.Failure("Recipient email address is required.");
            if (string.IsNullOrWhiteSpace(request.subject))
                return Result<bool>.Failure("Email subject is required.");
            if (string.IsNullOrWhiteSpace(request.body))
                return Result<bool>.Failure("Email body is required.");

            var emailMessage = new EmailMessage(request.from, request.to, request.subject, request.body);
           
            try
            {
                await _smtpRepository.SendEmailAsync(emailMessage, _smtpConfiguration, cancellationToken);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to send email: {ex.Message}");
            }
        }
    }
}
