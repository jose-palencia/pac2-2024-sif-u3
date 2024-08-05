using IShop.API.Dtos.Store;

namespace IShop.API.Services.Interfaces
{
    public interface IImagesService
    {
        Task<ImageDto> AddImage(IFormFile file);
        Task<string> DeleteImage(string publicId);
    }
}