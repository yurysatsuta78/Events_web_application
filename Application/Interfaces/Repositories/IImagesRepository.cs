using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IImagesRepository
    {
        Task AddImage(Image imageDomain, Guid eventId, CancellationToken cancellationToken);
        Task RemoveImage(int id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}