using MediatR;
using tns.Server.src.Modules.User.Aplication.Commands;
using tns.Server.src.Modules.User.Domain.Repositories;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.User.Aplication.Handlers
{
    public class DeleteUserCommnadHandler : IRequestHandler<DeleteUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        public DeleteUserCommnadHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            
            if (user == null)
                return Result<Guid>.Failure("User not found");
            
            await _userRepository.DeleteAsync(user);

            return Result<Guid>.Success(user.Id);
        }
    }
}
