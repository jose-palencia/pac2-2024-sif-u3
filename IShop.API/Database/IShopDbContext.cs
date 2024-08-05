using IShop.API.Database.Entities.Security;
using IShop.API.Database.Entities.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IShop.API.Database
{
    public class IShopDbContext : IdentityDbContext<UserEntity,
        RoleEntity,
        string,
        IdentityUserClaim<string>,
        UserRoleEntity,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>
    {
        public IShopDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserEntity>().ToTable("sec_users");

            builder.Entity<RoleEntity>().ToTable("sec_roles");

            builder.Entity<UserRoleEntity>().ToTable("sec_users_roles")
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.Entity<IdentityUserClaim<string>>().ToTable("sec_users_claims");

            builder.Entity<IdentityRoleClaim<string>>().ToTable("sec_roles_claims");

            builder.Entity<IdentityUserLogin<string>>().ToTable("sec_users_logins");

            builder.Entity<IdentityUserToken<string>>().ToTable("sec_users_tokens");


            //Set FKs OnRestrict no OnCascade
            var eTypes = builder.Model.GetEntityTypes();
            foreach (var type in eTypes)
            {
                var foreignKeys = type.GetForeignKeys();
                foreach (var foreignKey in foreignKeys)
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }
    
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductImageEntity> ProductsImages { get; set; }

    }
}