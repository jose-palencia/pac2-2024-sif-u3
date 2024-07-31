using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IShop.API.Database.Entities.Security
{
    public class UserRoleEntity : IdentityUserRole<string>
    {
        [DataType(DataType.DateTime)]
        public DateTime GrantedDate { get; set; }

        [StringLength(maximumLength: 450)]
        public string? GrantedById { get; set; }

        [ForeignKey(nameof(GrantedById))]
        public UserEntity? GrantedBy { get; set; }
    }
}