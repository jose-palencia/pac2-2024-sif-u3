namespace IShop.API.Dtos.Security.Auth
{
    public class LoginResponseDto
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }
        public List<string>? Roles { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}