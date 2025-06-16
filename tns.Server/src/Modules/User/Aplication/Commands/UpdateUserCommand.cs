using MediatR;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.User.Aplication.Commands
{
    public record UpdateUserCommand(Guid Id, string Name, string Email) : IRequest<Result<Guid>>;
}
