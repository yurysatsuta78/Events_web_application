using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public sealed class EventsRepository : BaseRepository<Event, Guid>, IEventsRepository
    {
        public EventsRepository(EventsDbContext dbContext) : base(dbContext) {}

        public async Task<IEnumerable<Event>> GetAllWithIncludesAsync(CancellationToken cancellationToken)
        {
            var eventModels = await _dbSet
                .Include(e => e.Participants)
                .Include(e => e.Images)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

            return eventModels;
        }

        public async Task<Event?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken)
        {
            var eventModel = await _dbSet
                .Include(e => e.Participants)
                .Include(e => e.Images)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return eventModel;
        }

        public async Task<Event?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var eventModel = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Name == name, cancellationToken);

            return eventModel;
        }
    }
}
