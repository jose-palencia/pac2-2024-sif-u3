using System.ComponentModel.DataAnnotations;

namespace IShop.API.Dtos.Security.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Correo Electrónico")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El {0} no tiene un formato válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La {0} es obligatoria")]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }
    }
}