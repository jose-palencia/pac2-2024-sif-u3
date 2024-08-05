using System.ComponentModel.DataAnnotations;

namespace IShop.API.Dtos.Store
{
    public class ImageDto
    {
        [StringLength(500)]
        public string? Url { get; set; }

        [StringLength(200)]
        public string? PublicId { get; set; }
    }
}