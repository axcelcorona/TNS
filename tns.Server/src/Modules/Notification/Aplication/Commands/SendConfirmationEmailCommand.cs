using MediatR;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.Notification.Aplication.Commands
{
    public record SendConfirmationEmailCommand(string token, string email) : IRequest<Result<bool>>;
}
