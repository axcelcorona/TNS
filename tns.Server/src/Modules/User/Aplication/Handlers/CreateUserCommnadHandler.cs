using MediatR;
using tns.Server.src.Modules.Notification.Aplication.Commands;
using tns.Server.src.Modules.User.Aplication.Commands;
using tns.Server.src.Modules.User.Domain.Repositories;
using tns.Server.src.Modules.User.Domain.Services;
using tns.Server.src.Shared.Mediator;

namespace tns.Server.src.Modules.User.Aplication.Handlers
{
    public class CreateUserCommnadHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;

        public CreateUserCommnadHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IMediator mediator, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mediator = mediator;
            _tokenService = tokenService;
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
                IsEmailConfirmed: false,
                updatedAt: null
            );

            // Save the user to the repository (assuming a method exists for this).
            await _userRepository.AddAsync(user);

            var welcomeEmailCommand = new SendWelcomEmailCommnad(
                to: request.Email,
                name: request.Name
            );

            var tokenConfirmation = _tokenService.GenerateTokenConfirmEmail(user);

            var confirmEmail = new SendConfirmationEmailCommand(token: tokenConfirmation, email: user.Email);

            _ = Task.Run(async () =>
            {
                try
                {
                    await _mediator.Send(welcomeEmailCommand);
                }
                catch (Exception ex)
                {
                    // Aquí puedes loggear el error si usas un logger
                    Console.WriteLine($"Error al enviar correo de bienvenida: {ex.Message}");
                }
            }, cancellationToken);

            _ = Task.Run(async () =>
            {
                try
                {
                    await _mediator.Send(confirmEmail);
                }
                catch (Exception ex)
                {
                    // Aquí puedes loggear el error si usas un logger
                    Console.WriteLine($"Error al enviar correo de confirmacion: {ex.Message}");
                }
            }, cancellationToken);

            return Result<Guid>.Success(user.Id);
        }
    }
}
