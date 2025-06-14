using MediatR;
using tns.Server.src.Modules.User.Aplication.Commands;
using tns.Server.src.Modules.User.Aplication.Queries;
using tns.Server.src.Modules.User.Domain.Repositories;
using tns.Server.src.Modules.User.Domain.Services;
using tns.Server.src.Modules.User.Infrastructure.Services;


namespace tns.Server.src.Modules.User.Aplication.Handlers
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<LoginResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public LoginUserQueryHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<Result<LoginResponse>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return Result<LoginResponse>.Failure("Invalid credentials");

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash, user.Salt))
                return Result<LoginResponse>.Failure("Invalid credentials");

            var token = _tokenService.GenerateToken(user);

            return Result<LoginResponse>.Success(new LoginResponse(user.Id, token));
        }
    }
}
