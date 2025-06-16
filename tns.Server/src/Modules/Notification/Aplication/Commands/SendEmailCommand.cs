using MediatR;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.Notification.Aplication.Commands
{
    public record SendEmailCommand(string from, string to, string subject, string body) : IRequest<Result<bool>>;
}
