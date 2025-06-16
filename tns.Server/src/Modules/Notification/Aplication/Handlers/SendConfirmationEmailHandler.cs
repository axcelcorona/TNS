using MediatR;
using Microsoft.Extensions.Options;
using System.Net;
using tns.Server.src.Modules.Notification.Aplication.Commands;
using tns.Server.src.Modules.Notification.Domain;
using tns.Server.src.Modules.Notification.Domain.Entities;
using tns.Server.src.Modules.Notification.Domain.Repositories;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.Notification.Aplication.Handlers
{
    public class SendConfirmationEmailHandler : IRequestHandler<SendConfirmationEmailCommand, Result<bool>>
    {

        private readonly INotificationSender _notificationSender;
        private readonly SmtpConfiguration _smtpConfiguration;

        public SendConfirmationEmailHandler(IOptions<SmtpConfiguration> options, INotificationSender notificationSender)
        {
            _smtpConfiguration = options.Value;
            _notificationSender = notificationSender;
        }

        public async Task<Result<bool>> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {

            var link = $"http://localhost:5000/api/user/confirm-email?token={request.token}&email={request.email}"; // Aquí puedes personalizar el enlace de confirmación

            var body = $@"
                    <!DOCTYPE html>
                    <html lang=""es"">
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Confirmación de Correo</title>
                    </head>
                    <body style=""font-family: Arial, sans-serif; padding: 20px;"">
                        <h1>¡Hola!</h1>
                        <p>Gracias por registrarte en nuestro servicio. Por favor, confirma tu correo electrónico haciendo clic en el siguiente botón:</p>

                        <a href=""{link}"" style=""
                            display: inline-block;
                            padding: 12px 24px;
                            margin-top: 20px;
                            background-color: #007BFF;
                            color: white;
                            text-decoration: none;
                            border-radius: 5px;
                            font-weight: bold;
                        "">Confirmar Correo</a>

                        <p style=""margin-top: 30px;"">Si no solicitaste esta cuenta, puedes ignorar este mensaje.</p>
                    </body>
                    </html>";

            var message = new EmailMessage(
                from: _smtpConfiguration.Username,
                to: request.email,
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
}
