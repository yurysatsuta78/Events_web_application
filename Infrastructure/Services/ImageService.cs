using Microsoft.AspNetCore.Http;
using Application.Interfaces.Services;
using Domain.Models;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        public async Task<List<Image>> LoadImage(IFormFile[] imageFiles, string path, CancellationToken cancellationToken = default)
        {
            try
            {
                var images = new List<Image>();

                foreach (var imageFile in imageFiles) 
                {
                    var fileName = $"{Guid.NewGuid()}.jpg";
                    var imagePath = Path.Combine(path, fileName);

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
