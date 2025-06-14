using MediatR;
using tns.Server.src.Modules.User.Aplication.Commands;
using tns.Server.src.Modules.User.Aplication.DTOs;
using tns.Server.src.Modules.User.Aplication.Queries;
using tns.Server.src.Modules.User.Domain.Repositories;

namespace tns.Server.src.Modules.User.Aplication.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                return Result<UserDto>.NotFound("User not found");
            }
            var userDto = new UserDto(user.Id, user.Name, user.Email)
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
            };
            return Result<UserDto>.Success(userDto);
        }
    }
}
