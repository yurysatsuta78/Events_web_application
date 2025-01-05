using Microsoft.AspNetCore.Http;
using Domain.Models;
using Application.Services;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly string _staticFilesPath =
            Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images");

        public async Task<List<Image>> LoadImages(IFormFile[] imageFiles, CancellationToken cancellationToken = default)
        {
            try
            {
                var images = new List<Image>();

                foreach (var imageFile in imageFiles) 
                {
                    var fileName = $"{Guid.NewGuid()}.jpg";
                    var imagePath = Path.Combine(_staticFilesPath, fileName);

                    await using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream, cancellationToken);
                    }

                    images.Add(Image.Create(imagePath));
                }

                return images;
            }
            catch
            {
                throw;
            }
        }
    }
}
