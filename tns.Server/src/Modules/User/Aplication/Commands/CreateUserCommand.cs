using MediatR;

namespace tns.Server.src.Modules.User.Aplication.Commands
{
    public record CreateUserCommand(string Name, string Email, string Password) : IRequest<Result<Guid>>;
}
