using Application.Interfaces.Repositories;
using Domain.Interfaces;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventsDbContext _dbContext;
        public IEventsRepository EventsRepository { get; }
        public IParticipantsRepository ParticipantsRepository { get; }
        public IRolesRepository RolesRepository { get; }

        public UnitOfWork(EventsDbContext dbContext, IEventsRepository eventsRepository,
            IParticipantsRepository participantsRepository, IRolesRepository rolesRepository)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            EventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));
            ParticipantsRepository = participantsRepository ?? throw new ArgumentNullException(nameof(participantsRepository));
            RolesRepository = rolesRepository ?? throw new ArgumentNullException(nameof(rolesRepository));
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
