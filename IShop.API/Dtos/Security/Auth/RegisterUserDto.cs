using System.ComponentModel.DataAnnotations;

namespace IShop.API.Dtos.Security.Auth
{
    public class RegisterUserDto
    {
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} letras.")]
        public string? FirstName { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} letras.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [Display(Name = "Correo Electrónico")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El {0} no tiene un formato válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La {0} es obligatoria")]
        [Display(Name = "Contraseña")]
        [RegularExpression(
            "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#\\$%\\^&\\*])(?=.*[a-zA-Z]).{8,}$", 
            ErrorMessage = "La {0} debe tener al menos 8 caracteres, incluyendo una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "La {0} es obligatoria")]
        [Display(Name = "Confirmación de Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        public string? ConfirmPassword { get; set; }
    }
}
