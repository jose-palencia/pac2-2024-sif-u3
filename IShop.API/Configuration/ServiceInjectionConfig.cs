using IShop.API.Helpers;
using IShop.API.Services.Interfaces;
using IShop.API.Services.Security;

namespace IShop.API.Configuration
{
    public static class ServiceInjectionConfig
    {
        public static void AddServiceInjections(this IServiceCollection services)
        {

            //services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddHttpContextAccessor();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IImagesService, ImagesService>();

            services.AddAutoMapper(typeof(AutoMapperProfiles));

        }
    }
}