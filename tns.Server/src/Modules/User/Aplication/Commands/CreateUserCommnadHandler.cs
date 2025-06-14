using MediatR;
using tns.Server.src.Modules.User.Domain.Repositories;
using tns.Server.src.Modules.User.Domain.Services;

namespace tns.Server.src.Modules.User.Aplication.Commands
{
    public class CreateUserCommnadHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommnadHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                return Result<Guid>.Failure("User with this email already exists.");
            }

            var salt = _passwordHasher.GenerateSalt();

            var passwordHash = _passwordHasher.HashPassword(request.Password, salt);

            var user = new Domain.User(
                id: Guid.NewGuid(),
                name: request.Name,
                email: request.Email,
                passwordHash: passwordHash,
                salt: salt,
                createdAt: DateTime.UtcNow,
                updatedAt: null
            );

            // Save the user to the repository (assuming a method exists for this).
            await _userRepository.AddAsync(user);

            return Result<Guid>.Success(user.Id);
        }
    }
}
