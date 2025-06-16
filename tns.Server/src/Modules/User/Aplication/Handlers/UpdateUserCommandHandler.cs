using MediatR;
using tns.Server.src.Modules.User.Aplication.Commands;
using tns.Server.src.Modules.User.Domain.Repositories;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.User.Aplication.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<Guid>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            
            if (user == null)
                return Result<Guid>.Failure("User not found");

            user.Update(request.Name, request.Email);
            
             await _userRepository.UpdateAsync(user);
            
            return Result<Guid>.Success(user.Id);
        }
    }
}
