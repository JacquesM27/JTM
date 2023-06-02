using FluentValidation;
using JTM.CQRS.Command.Account.ChangePassowrdUser;
using JTM.CQRS.Command.Account.ConfirmAccountuser;
using JTM.CQRS.Command.Account.RegisterUser;
using JTM.CQRS.Command.ForgetPassowrdUser;
using JTM.CQRS.Command.LoginUser;
using JTM.CQRS.Command.RefreshActivationTokenUser;
using JTM.CQRS.Command.RefreshTokenUser;
using JTM.DTO.Account;
using JTM.DTO.Account.RegisterUser;
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
        private readonly IValidator<RegisterUserDto> _validator;

        public AccountController(
            IMediator mediator,
            IValidator<RegisterUserDto> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterUser(RegisterUserDto request)
        {
            _validator.ValidateAndThrow(request);

            var command = new RegisterUserCommand
            {
                Email = request.Email,
                Password = request.Password,
                UserName = request.UserName
            };
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(UserDto userDto)
        {
            var command = new LoginUserCommand
            {
                Email = userDto.Email,
                Password = userDto.Password,
            };
            var response = await _mediator.Send(command);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken()
        {
            var command = new RefreshTokenUserCommand();

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        [Route("forget-password")]
        public async Task<ActionResult<AuthResponseDto>> ForgetPassword(string email)
        {
            var command = new ForgetPasswordUserCommand
            {
                Email = email,
            };

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                return Ok();
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        [Route("confirm")]
        public async Task<ActionResult<AuthResponseDto>> ConfirmAccount(int userId, string token)
        {
            var command = new ConfirmAccountCommand
            {
                UserId = userId,
                Token = token
            };

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                return Ok();
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        [Route("confirm-refresh")]
        public async Task<ActionResult<AuthResponseDto>> RefreshConfirmToken(string email)
        {
            var command = new RefreshActivationTokenUserCommand
            { 
                Email = email,
            };

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                return Ok();
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto request)
        {
            var command = new ChangePasswordCommand
            {
                Password = request.Password,
                Token = request.Token,
                UserId = request.UserId
            };

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                return Ok();
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        [Route("banhammer")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> BanUser(int userId)
        {

        }
    }
}
