using MediatR;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.Notification.Aplication.Commands
{
    public record SendWelcomEmailCommnad(string to, string name) : IRequest<Result<bool>>; 
}
