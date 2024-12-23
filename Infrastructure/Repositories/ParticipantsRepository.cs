using Application.DTO.Response.Participant;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public sealed class ParticipantsRepository : IParticipantsRepository
    {
        private readonly EventsDbContext _dbContext;
        private readonly IMapper _mapper;

        public ParticipantsRepository(EventsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task AddParticipant(Participant participantDomain, CancellationToken cancellationToken)
        {
            var participantEntity = _mapper.Map<ParticipantDb>(participantDomain);

            _dbContext.AttachRange(participantEntity.Roles);

            await _dbContext.Participants.AddAsync(participantEntity, cancellationToken);
        }

        public async Task DeleteParticipant(Guid id, CancellationToken cancellationToken)
        {
            var participantEntity = await _dbContext.Participants
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (participantEntity == null)
            {
                throw new EntityNotFoundException($"Participant not found.");
            }

            _dbContext.Participants.Remove(participantEntity);
        }

        public async Task<IEnumerable<GetParticipantDto>> GetAllParticipants(CancellationToken cancellationToken)
        {
            var participantEntities = await _dbContext.Participants
                .AsNoTracking()
                .Include(e => e.Events)
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

            var participantDtos = _mapper.Map<IEnumerable<GetParticipantDto>>(participantEntities);

            return participantDtos;
        }

        public async Task<Participant> GetParticipantByEmail(string email, CancellationToken cancellationToken)
        {
            var participantEntity = await _dbContext.Participants
                .AsNoTracking()
                .Include(e => e.Roles)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Email == email, cancellationToken);

            if (participantEntity == null)
            {
                throw new EntityNotFoundException($"Participant not found.");
            }

            return _mapper.Map<Participant>(participantEntity);
        }

        public async Task<Participant> GetParticipantById(Guid id, CancellationToken cancellationToken)
        {
            var participantEntity = await _dbContext.Participants
                .AsNoTracking()
                .Include(e => e.Roles)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (participantEntity == null)
            {
                throw new EntityNotFoundException($"Participant not found.");
            }

            return _mapper.Map<Participant>(participantEntity);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
