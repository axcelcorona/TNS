using MediatR;
using Microsoft.Extensions.Options;
using tns.Server.src.Modules.Notification.Aplication.Commands;
using tns.Server.src.Modules.Notification.Domain;
using tns.Server.src.Modules.Notification.Domain.Entities;
using tns.Server.src.Modules.Notification.Domain.Repositories;
using tns.Server.src.Shared.Mediator;

public class SentWelcomEmailHandler : IRequestHandler<SendWelcomEmailCommnad, Result<bool>>
{
    private readonly INotificationSender _notificationSender;
    private readonly SmtpConfiguration _smtpConfiguration;

    public SentWelcomEmailHandler(INotificationSender notificationSender, IOptions<SmtpConfiguration> options)
    {
        _notificationSender = notificationSender;
        _smtpConfiguration = options.Value; // <--- Esto NO es null si todo está bien
    }

    public async Task<Result<bool>> Handle(SendWelcomEmailCommnad request, CancellationToken cancellationToken)
    {
        var body = $@"
        <!DOCTYPE html>
        <html lang=""es"">
        <head>
            <meta charset=""UTF-8"">
            <title>Bienvenida</title>
        </head>
        <body>
            <h1>Hola <strong>{request.name}</strong>,</h1>
            <p>Welcome to our service!</p>
        </body>
        </html>";

        var message = new EmailMessage(
            from: _smtpConfiguration.Username,
            to: request.to,
            subject: "Bienvenido a nuestro servicio",
            body: body
        );

        try
        {
            await _notificationSender.SendEmailAsync(message, _smtpConfiguration, cancellationToken);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error sending email: {ex.Message}");
        }
    }

}
