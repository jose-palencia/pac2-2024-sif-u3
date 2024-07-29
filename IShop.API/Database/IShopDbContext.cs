using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IShop.API.Database
{
    public class IShopDbContext : IdentityDbContext<IdentityUser>
    {
        public IShopDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}