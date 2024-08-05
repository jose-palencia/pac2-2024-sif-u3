using IShop.API.Constants;
using IShop.API.Database.Entities.Security;
using IShop.API.Database.Entities.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IShop.API.Database
{
    public class IShopDbSeeder
    {
        public static async Task LoadDataAsync(
            UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,
            ILoggerFactory loggerFactory,
            IShopDbContext dbContext
        )
        {
            var logger = loggerFactory.CreateLogger<IShopDbSeeder>();
            try
            {
                await LoadRolesAsync(roleManager, loggerFactory);
                await LoadUsersAsync(userManager, roleManager, loggerFactory);
                await LoadProductsAsync(dbContext, loggerFactory);
                await LoadProductsImagesAsync(dbContext, loggerFactory);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error al inicializar datos con Seed.");
            }
        }

        private static async Task LoadProductsImagesAsync(IShopDbContext dbContext, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<IShopDbSeeder>();

            try
            {
                var jsonFilePath = "SeedFiles/products_images.json";

                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);

                var productsImages = JsonConvert.DeserializeObject<List<ProductImageEntity>>(jsonContent);

                if (!dbContext.ProductsImages.Any())
                {
                    dbContext.AddRange(productsImages!);
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation("Cargado seed de Imágenes.");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error al cargar imágenes desde el archivo JSON.");
            }
        }

        private static async Task LoadProductsAsync(IShopDbContext dbContext, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<IShopDbSeeder>();

            try
            {
                var jsonFilePath = "SeedFiles/products.json";

                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);

                var products = JsonConvert.DeserializeObject<List<ProductEntity>>(jsonContent);

                if (!dbContext.Products.Any())
                {
                    dbContext.AddRange(products!);
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation("Cargado seed de Productos.");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error al cargar productos desde el archivo JSON.");
            }
        }

        private static async Task LoadRolesAsync(RoleManager<RoleEntity> roleManager, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<IShopDbSeeder>();

            try
            {
                var jsonFilePath = "SeedFiles/roles.json";

                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);

                var roles = JsonConvert.DeserializeObject<List<RoleEntity>>(jsonContent);

                if (!roleManager.Roles.Any())
                {
                    foreach (var role in roles!)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name!))
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }
                    logger.LogInformation("Cargado seed de roles.");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error al cargar roles desde el archivo JSON.");
            }
        }

        private static async Task LoadUsersAsync(UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<IShopDbSeeder>();

            try
            {
                var jsonFilePath = "SeedFiles/users.json";

                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);

                var users = JsonConvert.DeserializeObject<List<UserEntity>>(jsonContent);

                if (!userManager.Users.Any())
                {
                    foreach (var user in users!)
                    {
                        var result = await userManager.CreateAsync(user, "Temporal01*");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, RolesConstant.ADMIN);
                        }
                    }
                    logger.LogInformation("Cargado seed de usuarios.");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error al cargar usuarios desde el archivo JSON.");
            }
        }
    }
}