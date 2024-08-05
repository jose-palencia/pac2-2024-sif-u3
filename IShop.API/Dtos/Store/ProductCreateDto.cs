using System.ComponentModel.DataAnnotations;

namespace IShop.API.Dtos.Store
{
    public class ProductCreateDto
    {
        [Display(Name = "Titulo")]
        [StringLength(200)]
        [Required]
        public string? Title { get; set; }

        [Display(Name = "Precio")]
        public decimal Price { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(200)]
        public string? Description { get; set; }

        [Display(Name = "Existencia")]
        public int Stock { get; set; }

        public IFormFile[]? ImagesList { get; set; }
    }
}