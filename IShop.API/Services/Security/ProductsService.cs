using AutoMapper;
using IShop.API.Database;
using IShop.API.Database.Entities.Store;
using IShop.API.Dtos.Common;
using IShop.API.Dtos.Store;
using IShop.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IShop.API.Services.Security
{
    public class ProductsService : IProductsService
    {
        private readonly IShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsService> _logger;
        private readonly IImagesService _imagesService;
        private readonly int PAGE_SIZE;

        public ProductsService(
            IShopDbContext context,
            IMapper mapper,
            ILogger<ProductsService> logger,
            IImagesService imagesService,
            IConfiguration configuration
        )
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _imagesService = imagesService;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
        }

        public async Task<ResponseDto<PaginationDto<List<ProductDto>>>> GetListAsync(string searchTerm = "", int page = 1)
        {
            try
            {
                int startIndex = (page - 1) * PAGE_SIZE;

                var productsQuery = _context.Products
                .Include(p => p.Images)
                    .Where(p => p.Title!.ToLower().Contains(searchTerm.ToLower()));

                int totalClients = await productsQuery.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalClients / PAGE_SIZE);

                var products = await productsQuery
                    .OrderByDescending(u => u.Title)
                    .Skip(startIndex)
                    .Take(PAGE_SIZE)
                    .ToListAsync();

                var productsDto = _mapper.Map<List<ProductDto>>(products);

                return new ResponseDto<PaginationDto<List<ProductDto>>>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Lista de registros obtenida correctamente",
                    Data = new PaginationDto<List<ProductDto>>
                    {
                        CurrentPage = page,
                        PageSize = PAGE_SIZE,
                        TotalItems = totalClients,
                        TotalPages = (int)Math.Ceiling((double)totalClients / PAGE_SIZE),
                        Items = productsDto,
                        HasPreviousPage = page > 1,
                        HasNextPage = page < totalPages
                    }
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,ex.Message);

                return new ResponseDto<PaginationDto<List<ProductDto>>>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Ocurrió un error al obtener la lista de registros."
                };
            }
        }

        public Task<ResponseDto<ProductDto>> GeyOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<ProductDto>> CreateAsync(ProductCreateDto dto)
        {
            try
            {
                var productEntity = _mapper.Map<ProductEntity>(dto);

                _context.Products.Add(productEntity);

                await _context.SaveChangesAsync();

                if(dto.ImagesList is not null && dto.ImagesList.Length > 0) 
                {
                    var productImagesEntity = new List<ProductImageEntity>();
                    foreach (var item in dto.ImagesList)
                    {
                        var imageDto = await _imagesService.AddImage(item);
                        productImagesEntity.Add(new ProductImageEntity{
                            Id = Guid.NewGuid(),
                            ProductId = productEntity.Id,
                            PublicId = imageDto.PublicId,
                            Url = imageDto.Url,
                        });
                    }

                    _context.AddRange(productImagesEntity);
                    await _context.SaveChangesAsync();
                }

                return new ResponseDto<ProductDto>
                {
                    Status = true,
                    StatusCode = 201,
                    Message = "Registro creado."
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);

                return new ResponseDto<ProductDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Ocurrió un error al crear el registro."
                };
            }
        }

        public Task<ResponseDto<ProductDto>> EditAsync(ProductEditDto dto, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<ProductDto>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}