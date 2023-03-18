using JTM.DTO;
using JTM.Services.AuthService;
using JTM.Services.MailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JTM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;

        public AccountController(IAuthService authService, IMailService mailService)
        {
            _authService = authService;
            _mailService = mailService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<AuthResponseDto>> RegisterUser(UserRegisterDto request)
        {
            var response = await _authService.RegisterUser(request);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            await _mailService.SendConfirmationEmail(request.Email);
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
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(UserDto userDto)
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
            await _mailService.SendPasswordResetEmail(email);
            return Ok();
        }

        [HttpPost]
        [Route("confirm")]
        public async Task<ActionResult> ConfirmAccount(int userId, string token)
        {
            var response = await _authService.ConfirmAccount(userId, token);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("refresh-confirm")]
        public async Task<ActionResult> RefreshConfirmToken(string email)
        {
            var response = await _authService.RefreshActivationToken(email);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            await _mailService.SendConfirmationEmail(email);
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
    }
}
