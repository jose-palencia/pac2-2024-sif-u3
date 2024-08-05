using System.ComponentModel.DataAnnotations.Schema;

namespace IShop.API.Database.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        public Guid Id { get; set; }
    }
}