using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IShop.API.Database.Entities.Security
{
    public class RoleEntity : IdentityRole
    {
        [StringLength(maximumLength: 250, MinimumLength = 3)]
        public string? Description { get; set; }
    }
}