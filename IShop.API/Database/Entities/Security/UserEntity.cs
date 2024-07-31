
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IShop.API.Database.Entities.Security
{
    public class UserEntity : IdentityUser
    {
        [Display(Name = "Nombres")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} letras.")]
        public string? FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} letras.")]
        public string? LastName { get; set; }

        [Display(Name = "Estado")]
        [Range(minimum: 0, maximum: 1, ErrorMessage = "Solo de acepta uno ó cero.")]
        public int Status { get; set; }

        public string? PasswordLoss { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PasswordLossDate { get; set; }

        [Display(Name = "Genero")]
        [Column("gender")]
        [StringLength(maximumLength: 1, ErrorMessage = "El campo {0} debe tener entre {1} letras.")]
        public string? Gender { get; set; }

        [DataType(DataType.DateTime)]
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("address")]
        [StringLength(maximumLength: 250, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} letras.")]
        public string? Address { get; set; }

        [Column("job_title")]
        [StringLength(maximumLength: 250, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} letras.")]
        public string? JobTitle { get; set; }

        [Column("identity")]
        [StringLength(maximumLength: 250, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} letras.")]
        public string? Identity { get; set; }

        [Column("refresh_token")]
        [StringLength(300)]
        public string? RefreshToken { get; set; }

        [Column("refresh_token_expiry")]
        public DateTime RefreshTokenExpiry { get; set; }

        [ConcurrencyCheck]
        public Guid Version { get; set; }

    }
}