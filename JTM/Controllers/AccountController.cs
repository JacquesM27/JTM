using JTM.CQRS.Command.Auth.RegisterUser;
using JTM.DTO.Account;
using JTM.DTO.Account.RegisterUser;
using JTM.Services.AuthService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JTM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService, IMediator mediator)
        {
            _authService = authService;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<AuthResponseDto>> RegisterUser(RegisterUserDto request)
        {
            var command = new RegisterUserCommand
            {
                Email = request.Email,
                Password = request.Password,
                UserName = request.UserName
            };
            int id = await _mediator.Send(command);
            //var response = await _authService.RegisterUser(request);
            //if (!response.Success)
            //{
            //    return BadRequest(response.Message);
            //}
            //await _mailService.SendConfirmationEmail(request.Email);
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(UserDto userDto)
        {
            var response = await _authService.Login(userDto);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken()
        {
            var response = await _authService.RefreshToken();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        [Route("forget-password")]
        public async Task<ActionResult> ForgetPassword(string email)
        {
            var response = await _authService.ForgetPassword(email);
            if(!response.Success)
            {
                return BadRequest(response.Message);
            }
            //await _mailService.SendPasswordResetEmail(email);
            return Ok();
        }

        [HttpPost]
        [Route("confirm")]
        public async Task<ActionResult> ConfirmAccount(int userId, string token)
        {
            if (userId == 0)
            {
                return BadRequest("Missing userId");
            }
            var response = await _authService.ConfirmAccount(userId, token);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("confirm-refresh")]
        public async Task<ActionResult> RefreshConfirmToken(string email)
        {
            var response = await _authService.RefreshActivationToken(email);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            //await _mailService.SendConfirmationEmail(email);
            return Ok();
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto request)
        {
            var response = await _authService.ChangePassword(request);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok();
        }

        //TODO ban user
    }
}
