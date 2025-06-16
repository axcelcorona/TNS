using MediatR;
using tns.Server.src.Modules.User.Aplication.DTOs;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.User.Aplication.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;
}
