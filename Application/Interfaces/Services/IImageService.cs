using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IImageService
    {
        Task<List<Image>> LoadImage(IFormFile[] imageFile, string path, CancellationToken cancellationToken);
    }
}