using MediatR;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.User.Aplication.Commands
{
    public record UpdateUserPasswordCommnad(Guid id, string oldPassword, string newPassword): IRequest<Result<bool>>;
}
