using MediatR;
using tns.Server.src.Modules.User.Aplication.Commands;
using tns.Server.src.Modules.User.Domain.Repositories;
using tns.Server.src.Modules.User.Domain.Services;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.User.Aplication.Handlers
{
    public class ConfirmationEmailHandler : IRequestHandler<ConfirmationEmailCommand, Result<bool>>
    {

        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public ConfirmationEmailHandler(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> Handle(ConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.email);

            if(!_tokenService.ValidateToken(request.token)) return Result<bool>.Failure("Invalid token.");
            if(user == null) return Result<bool>.Failure("User with this email does not exist.");

            user.UpdateConfirmEmail();

            await _userRepository.UpdateAsync(user);

            return Result<bool>.Success(true);
        }
    }
}
