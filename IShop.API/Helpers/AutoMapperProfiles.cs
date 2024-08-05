using AutoMapper;
using IShop.API.Database.Entities.Store;
using IShop.API.Dtos.Store;

namespace IShop.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            MapsForProducts();
        }

        private void MapsForProducts()
        {
            CreateMap<ProductEntity, ProductDto>()
                       .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images!.Select(pt => pt.Url).ToList()));
            CreateMap<ProductCreateDto, ProductEntity>();
            CreateMap<ProductEditDto, ProductEntity>();
        }
    }
}