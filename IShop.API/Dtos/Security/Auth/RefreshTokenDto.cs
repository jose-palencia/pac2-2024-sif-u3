using System.ComponentModel.DataAnnotations;

namespace IShop.API.Dtos.Security.Auth
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "El token es requerido")]
        public string? Token { get; set; }

        [Required(ErrorMessage = "El refresh token es requerido")]
        public string? RefreshToken { get; set; }
    }
}