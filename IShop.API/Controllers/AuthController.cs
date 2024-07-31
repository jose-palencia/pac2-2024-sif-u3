using IShop.API.Dtos.Common;
using IShop.API.Dtos.Security.Auth;
using IShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IShop.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService,
            IConfiguration configuration)
        {
            this._authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<NoContent>>> Register(RegisterUserDto dto)
        {
            var loginResponse = await _authService.RegisterAsync(dto);

            return StatusCode(loginResponse.StatusCode, new
            {
                loginResponse.Status,
                loginResponse.Message,
                loginResponse.Data
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<LoginResponseDto>>> Login(LoginDto dto)
        {
            var loginResponse = await _authService.LoginAsync(dto);

            return StatusCode(loginResponse.StatusCode, new
            {
                loginResponse.Status,
                loginResponse.Message,
                loginResponse.Data
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<LoginResponseDto>>> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var loginResponseDto = await _authService.RefreshTokenAsync(dto);

            return StatusCode(loginResponseDto.StatusCode, new
            {
                Status = true,
                loginResponseDto.Message,
                loginResponseDto.Data
            });
        }

    }
}