using FluentValidation;
using JTM.CQRS.Command.Account;
using JTM.DTO.Account;
using JTM.DTO.Account.RegisterUser;
using JTM.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JTM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<RegisterUserDto> _validator;

        public AccountController(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
            IValidator<RegisterUserDto> validator)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterUser(RegisterUserDto request)
        {
            _validator.ValidateAndThrow(request);

            var command = new RegisterCommand(
                userName: request.UserName,
                email: request.Email,
                password: request.Password,
                UserRole.user);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("registerAdmin")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> RegisterAdmin(RegisterUserDto request)
        {
            _validator.ValidateAndThrow(request);

            var command = new RegisterCommand(
                userName: request.UserName,
                email: request.Email,
                password: request.Password,
                UserRole.admin);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(UserDto userDto)
        {
            var command = new LoginCommand(
              email: userDto.Email,
              password: userDto.Password);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken()
        {
            var refreshToken = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
            var command = new RefreshTokenCommand(refreshToken);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost]
        [Route("forget-password")]
        public async Task<ActionResult> ForgetPassword(EmailDto request)
        {
            var command = new ForgetPasswordCommand(email: request.Email);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("confirm")]
        public async Task<ActionResult> ConfirmAccount(ConfirmDto request)
        {
            var command = new ConfirmAccountCommand(
                userId: request.UserId,
                token: request.Token);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("confirm-refresh")]
        public async Task<ActionResult<AuthResponseDto>> RefreshConfirmToken(EmailDto request)
        {
            var command = new RefreshConfirmTokenCommand(email: request.Email);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto request)
        {
            var command = new ChangePasswordCommand(
                userId: request.UserId,
                password: request.Password,
                token: request.Token);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("banhammer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<int>> BanUser(int userId)
        {
            var command = new BanCommand(userId: userId);
            int result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost]
        [Route("unban")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<int>> UnbanUser(int userId)
        {
            var command = new UnbanCommand(userId: userId);
            int result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
