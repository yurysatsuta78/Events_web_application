using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public interface IImageService
    {
        Task<List<Image>> LoadImages(IFormFile[] imageFile, CancellationToken cancellationToken);
    }
}