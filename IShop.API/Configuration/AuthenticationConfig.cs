using System.Text;
using IShop.API.Database;
using IShop.API.Database.Entities.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace IShop.API.Configuration
{
    public static class AuthenticationConfig
    {
        public static void AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<UserEntity, RoleEntity>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Tokens.EmailConfirmationTokenProvider = "EmailConfirmation";
            }).AddEntityFrameworkStores<IShopDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
                };
            });
        }
    }
}