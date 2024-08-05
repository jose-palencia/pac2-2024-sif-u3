using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IShop.API.Database.Entities.Store
{
    [Table("str_products_images")]
    public class ProductImageEntity : BaseEntity
    {
        [StringLength(500)]
        public string? Url { get; set; }

        [StringLength(200)]
        public string? PublicId { get; set; }

        public Guid ProductId { get; set; }
        
        [ForeignKey(nameof(ProductId))]
        public virtual ProductEntity? Product { get; set; }
    }
}