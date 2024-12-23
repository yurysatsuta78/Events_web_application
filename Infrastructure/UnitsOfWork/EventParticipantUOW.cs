using Infrastructure.Repositories;
using Application.Interfaces.UnitsOfWork;
using AutoMapper;
using Application.Interfaces.Repositories;

namespace Infrastructure.UnitsOfWork
{
    public class EventParticipantUOW : IEventParticipantUOW
    {
        private readonly EventsDbContext _dbContext;
        private readonly IMapper _mapper;

        public EventParticipantUOW(EventsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            EventsRepository = new EventsRepository(_dbContext, _mapper);
            ParticipantsRepository = new ParticipantsRepository(_dbContext, _mapper);
        }

        public IEventsRepository EventsRepository { get; }
        public IParticipantsRepository ParticipantsRepository { get; }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
