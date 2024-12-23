using Application.Interfaces.Repositories;
using Application.Interfaces.UnitsOfWork;
using AutoMapper;
using Infrastructure.Repositories;

namespace Infrastructure.UnitsOfWork
{
    public class ParticipantRoleUOW : IParticipantRoleUOW
    {
        private readonly EventsDbContext _dbContext;
        private readonly IMapper _mapper;

        public ParticipantRoleUOW(EventsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            ParticipantsRepository = new ParticipantsRepository(_dbContext, _mapper);
            RolesRepository = new RolesRepository(_dbContext, _mapper);
        }

        public IParticipantsRepository ParticipantsRepository { get; }
        public IRolesRepository RolesRepository { get; }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
