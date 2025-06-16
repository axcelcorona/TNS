using MediatR;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.User.Aplication.Queries
{
    public record LoginUserQuery(string Email, string Password) : IRequest<Result<LoginResponse>>;
    public record LoginResponse(Guid UserId, string Token);
}
