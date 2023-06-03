using FluentValidation;
using JTM.CQRS.Command.Account.ChangePassowrdUser;
using JTM.CQRS.Command.Account.RegisterUser;
using JTM.CQRS.Command.Account.ForgetPassowrdUser;
using JTM.CQRS.Command.Account.LoginUser;
using JTM.CQRS.Command.Account.RefreshActivationTokenUser;
using JTM.CQRS.Command.Account.RefreshTokenUser;
using JTM.DTO.Account;
using JTM.DTO.Account.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JTM.CQRS.Command.Account.ConfirmAccountUser;
using JTM.CQRS.Command.Account.BanUser;

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
            (
                userName: request.UserName,
                email: request.Email,
                password: request.Password
            );
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(UserDto userDto)
        {
            var command = new LoginCommand
            (
              email: userDto.Email,
              password: userDto.Password
            );

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
            var command = new RefreshTokenCommand();

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
            var command = new ForgetPasswordCommand
            (
                email: email
            );

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
            (
                userId: userId,
                token: token
            );

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
            var command = new RefreshConfirmTokenCommand
            ( 
                email: email
            );

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
            (
                userId: request.UserId,
                password: request.Password,
                token: request.Token
            );

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
            var command = new BanUserCommand
            (
                userId: userId
            );
            
            var response = await _mediator.Send(command);
            if (response.Success)
            {
                return Ok();
            }
            return BadRequest(response.Message);
        }
    }
}
