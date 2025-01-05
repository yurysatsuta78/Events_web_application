using Domain.Interfaces.Repositories;
using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IImagesRepository : IBaseRepository<Image, int>
    {
    }
}