using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IShop.API.Database;
using IShop.API.Database.Entities.Security;
using IShop.API.Dtos.Common;
using IShop.API.Dtos.Security.Auth;
using IShop.API.Helpers;
using IShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IShop.API.Services.Security
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IShopDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            SignInManager<UserEntity> signInManager,
            UserManager<UserEntity> userManager,
            IConfiguration configuration,
            IShopDbContext context,
            ILogger<AuthService> logger
        )
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._configuration = configuration;
            this._context = context;
            this._logger = logger;
        }

        public async Task<ResponseDto<NoContent>> RegisterAsync(RegisterUserDto dto)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(dto.Email!);
                if (userExists != null)
                    return new ResponseDto<NoContent>
                    {
                        Status = false,
                        StatusCode = 400,
                        Message = "El usuario ya existe."
                    };

                var user = new UserEntity
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    UserName = dto.Email,
                    Status = 1 // Activado por defecto
                };

                var result = await _userManager.CreateAsync(user, dto.Password!);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new ResponseDto<NoContent>
                    {
                        Status = false,
                        StatusCode = 400,
                        Message = $"Error al crear el usuario: {errors}"
                    };
                }

                // Asignar roles u otras configuraciones necesarias

                return new ResponseDto<NoContent>
                {
                    Status = true,
                    StatusCode = 201,
                    Message = "Usuario registrado satisfactoriamente."
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<NoContent>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = $"Se ha producido un error al intentar registrar el usuario. Por favor, inténtelo de nuevo más tarde.",
                };
            }
        }

        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(dto.Email!, dto.Password!, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var userEntity = await _userManager.FindByEmailAsync(dto.Email!);

                    if (userEntity?.Status != 1)
                    {
                        return new ResponseDto<LoginResponseDto>
                        {
                            Status = false,
                            StatusCode = 401,
                            Message = $"El usuario no está activado, comuníquese con un administrador del sistema."
                        };
                    }

                    List<Claim> authClaims = await GetClaims(userEntity);

                    // Generate the token with the claims
                    var jwtToken = GetToken(authClaims);

                    var userRoles = await _userManager.GetRolesAsync(userEntity);

                    var loginResponseDto = new LoginResponseDto
                    {
                        Email = userEntity.Email,
                        FullName = $"{userEntity.FirstName} {userEntity.LastName}",
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        TokenExpiration = jwtToken.ValidTo,
                        Roles = [],
                    };

                    foreach (var role in userRoles)
                    {
                        loginResponseDto.Roles.Add(role);
                    }

                    var entry = _context.Entry(userEntity);
                    await entry.ReloadAsync();

                    loginResponseDto.RefreshToken = this.GenerateRefreshTokenString();

                    userEntity.RefreshToken = loginResponseDto.RefreshToken;
                    userEntity.RefreshTokenExpiry = DateTimeUtils.DateTimeNowHonduras().AddMinutes(int.Parse(_configuration["JWT:RefreshTokenExpiry"]!));
                    userEntity.Version = Guid.NewGuid();

                    entry.State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    // Response Ok
                    return new ResponseDto<LoginResponseDto>
                    {
                        Status = true,
                        StatusCode = 200,
                        Message = $"Inicio de sesión realizado satisfactoriamente.",
                        Data = loginResponseDto
                    };
                }
                else
                {
                    // Response Error
                    return new ResponseDto<LoginResponseDto>
                    {
                        Status = false,
                        StatusCode = 400,
                        Message = $"Se presentó un error durante el inicio de sesión, usuario o contraseña inválidos."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseDto<LoginResponseDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = $"Se ha producido un error al intentar iniciar sesión. Por favor, inténtelo de nuevo más tarde.",
                };
            }
        }

        private async Task<List<Claim>> GetClaims(UserEntity userEntity)
        {
            // ClaimList creation
            var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Email, userEntity.Email!),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Name, $"{userEntity.FirstName} {userEntity.LastName}"),
                    new("UserId", userEntity.Id),
                };

            var userRoles = await _userManager.GetRolesAsync(userEntity);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            return authClaims;
        }

        public async Task<ResponseDto<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
        {
            string email = "";
            try
            {
                // Obtener el principal del token proporcionado
                var principal = GetTokenPrincipal(dto.Token!);

                // Obtener el claim de email del token
                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

                // Verificar si el claim de email está presente
                if (emailClaim is null)
                {
                    return new ResponseDto<LoginResponseDto>
                    {
                        Status = true,
                        StatusCode = 401,
                        Message = "Acceso no autorizado: No se encontró el claim de correo electrónico en el token."
                    };
                }

                email = emailClaim.Value;

                // Buscar el usuario por correo electrónico
                var userEntity = await _userManager.FindByEmailAsync(email);

                // Verificar si el usuario existe, está activo, y si el refresh token es válido
                if (userEntity is null)
                {
                    return new ResponseDto<LoginResponseDto>
                    {
                        Status = true,
                        StatusCode = 401,
                        Message = "Acceso no autorizado: El usuario no existe."
                    };
                }

                if (userEntity.Status == 0)
                {
                    return new ResponseDto<LoginResponseDto>
                    {
                        Status = true,
                        StatusCode = 401,
                        Message = "Acceso no autorizado: El usuario está deshabilitado."
                    };
                }

                if (userEntity.RefreshToken != dto.RefreshToken)
                {
                    return new ResponseDto<LoginResponseDto>
                    {
                        Status = true,
                        StatusCode = 401,
                        Message = "Acceso no autorizado: La sesión no es válida."
                    };
                }

                if (userEntity.RefreshTokenExpiry < DateTimeUtils.DateTimeNowHonduras())
                {
                    return new ResponseDto<LoginResponseDto>
                    {
                        Status = true,
                        StatusCode = 401,
                        Message = "Acceso no autorizado: La sesión ha expirado."
                    };
                }

                List<Claim> authClaims = await GetClaims(userEntity);

                // Generar el nuevo token JWT
                var jwtToken = GetToken(authClaims);

                // Crear la respuesta con los datos del nuevo token y la información del usuario
                var loginResponseDto = new LoginResponseDto
                {
                    Email = userEntity.Email,
                    FullName = $"{userEntity.FirstName} {userEntity.LastName}",
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    TokenExpiration = jwtToken.ValidTo,
                    RefreshToken = this.GenerateRefreshTokenString(),
                    Roles = new List<string>(),
                };

                // Agregar roles del usuario a la respuesta
                var userRoles = await _userManager.GetRolesAsync(userEntity);
                foreach (var role in userRoles)
                {
                    loginResponseDto.Roles.Add(role);
                }

                // Actualizar el token de actualización y su fecha de expiración en el usuario
                var entry = _context.Entry(userEntity);
                await entry.ReloadAsync();

                loginResponseDto.RefreshToken = this.GenerateRefreshTokenString();

                userEntity.RefreshToken = loginResponseDto.RefreshToken;
                userEntity.RefreshTokenExpiry = DateTimeUtils.DateTimeNowHonduras().AddMinutes(int.Parse(_configuration["JWT:RefreshTokenExpiry"]!));
                userEntity.Version = Guid.NewGuid();

                entry.State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Devolver respuesta exitosa con el nuevo token
                return new ResponseDto<LoginResponseDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Token renovado satisfactoriamente.",
                    Data = loginResponseDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Ocurrió un error al renovar la sesión. Por favor, inténtelo nuevamente.",
                    Data = null
                };
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTimeUtils.DateTimeNowHonduras().AddMinutes(int.Parse(_configuration["JWT:Expires"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        public ClaimsPrincipal GetTokenPrincipal(string token)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value!));

            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = securityKey,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }
    }
}