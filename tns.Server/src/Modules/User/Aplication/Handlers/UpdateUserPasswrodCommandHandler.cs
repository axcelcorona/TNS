using tns.Server.src.Modules.User.Aplication.Commands;
using tns.Server.src.Modules.User.Domain.Repositories;
using tns.Server.src.Modules.User.Domain.Services;
using MediatR;

namespace tns.Server.src.Modules.User.Aplication.Handlers
{
    public class UpdateUserPasswrodCommandHandler : IRequestHandler<UpdateUserPasswordCommnad, Result<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UpdateUserPasswrodCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<Result<bool>> Handle(UpdateUserPasswordCommnad request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.id);
            
            if (user == null)
                return Result<bool>.Failure("User not found");

            if (!_passwordHasher.VerifyPassword(request.oldPassword, user.PasswordHash, user.Salt))
                return Result<bool>.Failure("Old password is incorrect");

            var newSalt = _passwordHasher.GenerateSalt();

            var newPasswordHash = _passwordHasher.HashPassword(request.newPassword, newSalt);

            user.UpdatePassword(newPasswordHash, newSalt);

            await _userRepository.UpdateAsync(user);
            
            return Result<bool>.Success(true);
        }
    }
}
