using IShop.API.Dtos.Common;
using IShop.API.Dtos.Store;

namespace IShop.API.Services.Interfaces
{
    public interface IProductsService
    {
        Task<ResponseDto<PaginationDto<List<ProductDto>>>> GetListAsync(string searchTerm = "", int page = 1);
        Task<ResponseDto<ProductDto>> GeyOneByIdAsync(Guid id);
        Task<ResponseDto<ProductDto>> CreateAsync(ProductCreateDto dto);
        Task<ResponseDto<ProductDto>> EditAsync(ProductEditDto dto, Guid id);
        Task<ResponseDto<ProductDto>> DeleteAsync(Guid id);
    }
}