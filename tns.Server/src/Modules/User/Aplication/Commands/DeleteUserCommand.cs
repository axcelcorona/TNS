using MediatR;

namespace tns.Server.src.Modules.User.Aplication.Commands
{
    public record DeleteUserCommand(Guid Id) : IRequest<Result<Guid>>;
}
