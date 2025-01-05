using Domain.Interfaces.Repositories;
using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IEventsRepository : IBaseRepository<Event, Guid>
    {
        Task<Event?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<Event?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Event>> GetAllWithIncludesAsync(CancellationToken cancellationToken);
    }
}