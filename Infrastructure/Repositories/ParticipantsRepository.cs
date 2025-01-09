using Application.Interfaces.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public sealed class ParticipantsRepository : BaseRepository<Participant, Guid>, IParticipantsRepository
    {
        public ParticipantsRepository(EventsDbContext dbContext) : base(dbContext) { }

        public async Task<Participant?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var participantModel = await _dbSet
                .Include(e => e.Roles)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Email == email, cancellationToken);

            return participantModel;
        }

        public async Task<Participant?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken)
        {
            var participantModel = await _dbSet
                .Include(e => e.Roles)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return participantModel;
        }
    }
}
