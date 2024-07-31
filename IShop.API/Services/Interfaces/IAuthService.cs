using System.Security.Claims;
using IShop.API.Dtos.Common;
using IShop.API.Dtos.Security.Auth;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IShop.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<NoContent>> RegisterAsync(RegisterUserDto dto);
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto);
        Task<ResponseDto<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto);
        ClaimsPrincipal GetTokenPrincipal(string token);
    }
}