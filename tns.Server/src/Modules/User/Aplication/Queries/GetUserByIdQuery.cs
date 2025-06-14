using MediatR;
using tns.Server.src.Modules.User.Aplication.Commands;
using tns.Server.src.Modules.User.Aplication.DTOs;

namespace tns.Server.src.Modules.User.Aplication.Queries
{
    public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;
}
