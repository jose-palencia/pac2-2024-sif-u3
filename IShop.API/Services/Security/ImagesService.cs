using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using IShop.API.Dtos.Common;
using IShop.API.Dtos.Store;
using IShop.API.Services.Interfaces;

namespace IShop.API.Services.Security
{
    public class ImagesService : IImagesService
    {
        private readonly ClaudinarySettings _claudinarySettings;
        private readonly Cloudinary _claudinary;
        public ImagesService(
            IConfiguration configuration
        )
        {
            _claudinarySettings = configuration.GetSection("ClaudinarySettings").Get<ClaudinarySettings>()!;

            var account = new Account(
                _claudinarySettings.CloudName,
                _claudinarySettings.ApiKey,
                _claudinarySettings.ApiSecret
            );

            _claudinary = new Cloudinary(account);
        }
        public async Task<ImageDto> AddImage(IFormFile file)
        {
            if(file.Length > 0) 
            {
                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _claudinary.UploadAsync(uploadParams);

                if(uploadResult.Error is not null) 
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                return new ImageDto
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl.ToString(),
                };
            }

            return null!;
        }

        public async Task<string> DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var deleteResult = await _claudinary.DestroyAsync(deleteParams);

            return deleteResult.Result == "ok" ? deleteResult.Result : null!;
        }
    }
}