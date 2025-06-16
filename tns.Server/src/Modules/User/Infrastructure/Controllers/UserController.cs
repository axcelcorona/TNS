using tns.Server.src.Modules.User.Aplication.Commands;
using tns.Server.src.Modules.User.Aplication.Queries;
using tns.Server.src.Shared.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace tns.Server.src.Modules.User.Infrastructure.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [BasicAuth]
        public async Task<IActionResult> Login([FromBody] LoginUserQuery request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess) return Unauthorized();

            Response.Cookies.Append("auth_token", result.Value.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(new { result.Value.UserId });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [BasicAuth]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetUserById), new { id = result.Value }, new { id = result.Value })
                : BadRequest(result.Error);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete(":id")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand(id);
            var result = await _mediator.Send(command);

            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordCommnad command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);

        }
    }
}
