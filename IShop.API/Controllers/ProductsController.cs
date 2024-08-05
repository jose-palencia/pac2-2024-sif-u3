using IShop.API.Dtos.Common;
using IShop.API.Dtos.Store;
using IShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IShop.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<ProductDto>>> GetAll(string searchTerm = "", int page = 1)
        {
            var response = await _productsService.GetListAsync(searchTerm, page);

            return StatusCode(response.StatusCode, new 
            {
                response.Status,
                response.Message,
                response.Data,
            }) ;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<ProductDto>>> Create([FromForm] ProductCreateDto dto)
        {
            var response = await _productsService.CreateAsync(dto);

            return StatusCode(response.StatusCode, new 
            {
                response.Status,
                response.Message,
                //response.Data,
            }) ;
        }
    }
}