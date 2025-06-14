using MediatR;

namespace tns.Server.src.Modules.User.Aplication.Commands
{
    public record UpdateUserPasswordCommnad(Guid id, string oldPassword, string newPassword): IRequest<Result<bool>>;
}
