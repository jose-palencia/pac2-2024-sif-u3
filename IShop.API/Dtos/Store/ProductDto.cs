namespace IShop.API.Dtos.Store
{
    public class ProductDto
    {
        public Guid Id { get; set;}
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public string[]? Images { get; set; }
    }
}