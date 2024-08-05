using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IShop.API.Database.Entities.Store
{
    [Table("str_products")]
    public class ProductEntity : BaseEntity
    {
        [StringLength(200)]
        [Required]
        public string? Title { get; set; }

        [Column(TypeName = "Decimal(15,2)")]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }
        public int Stock { get; set; }
        public virtual IEnumerable<ProductImageEntity>? Images { get; set; }
    }
}