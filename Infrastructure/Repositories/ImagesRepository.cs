using Application.Interfaces.Repositories;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public sealed class ImagesRepository : BaseRepository<Image, int>, IImagesRepository
    {
        public ImagesRepository(EventsDbContext dbContext) : base(dbContext) { }
    }
}
