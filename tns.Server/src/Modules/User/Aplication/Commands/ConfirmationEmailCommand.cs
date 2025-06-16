using MediatR;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.User.Aplication.Commands
{
    public record ConfirmationEmailCommand(string email, string token) : IRequest<Result<bool>>;

}
