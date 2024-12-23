using Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities;
using Domain.Exceptions;
using Application.Interfaces.Repositories;
using Application.DTO.Response.Event;

namespace Infrastructure.Repositories
{
    public sealed class EventsRepository : IEventsRepository
    {
        private readonly EventsDbContext _dbContext;
        private readonly IMapper _mapper;

        public EventsRepository(EventsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }



        public async Task AddEvent(Event eventDomain, CancellationToken cancellationToken)
        {
            var eventEntity = _mapper.Map<EventDb>(eventDomain);

            await _dbContext.Events.AddAsync(eventEntity, cancellationToken);
        }



        public async Task DeleteEvent(Guid id, CancellationToken cancellationToken)
        {
            var eventEntity = await _dbContext.Events
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (eventEntity == null)
            {
                throw new EntityNotFoundException($"Event not found.");
            }

            _dbContext.Events.Remove(eventEntity);
        }



        public async Task<IEnumerable<GetEventDto>> GetAllEvents(CancellationToken cancellationToken)
        {
            var eventEntities = await _dbContext.Events
                .AsNoTracking()
                .Include(e => e.Participants)
                .Include(e => e.Images)
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

            var eventsDtos = _mapper.Map<IEnumerable<GetEventDto>>(eventEntities);
            return eventsDtos;
        }



        public async Task<Event> GetEventById(Guid id, CancellationToken cancellationToken)
        {
            var eventEntity = await _dbContext.Events
                .AsNoTracking()
                .Include(e => e.Participants)
                .Include(e => e.Images)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (eventEntity == null)
            {
                throw new EntityNotFoundException($"Event not found.");
            }

            return _mapper.Map<Event>(eventEntity);
        }



        public async Task<Event> GetEventByName(string name, CancellationToken cancellationToken)
        {
            var eventEntity = await _dbContext.Events
                .AsNoTracking()
                .Include(e => e.Participants)
                .Include(e => e.Images)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Name == name, cancellationToken);

            if (eventEntity == null)
            {
                throw new EntityNotFoundException($"Event not found.");
            }

            return _mapper.Map<Event>(eventEntity);
        }



        public async Task UpdateEvent(Event eventDomain, CancellationToken cancellationToken)
        {
            var eventEntity = await _dbContext.Events
                .FirstAsync(e => e.Id == eventDomain.Id, cancellationToken);

            _mapper.Map(eventDomain, eventEntity);


            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                Console.WriteLine($"{entry.Entity.GetType().Name} {entry.State}");
            }
        }



        public async Task UpdateEventParticipants(Event eventDomain, CancellationToken cancellationToken) 
        {
            var eventEntity = await _dbContext.Events
                .Include(e => e.Participants)
                .FirstAsync(e => e.Id == eventDomain.Id, cancellationToken);

            var domainCollectionIds = eventDomain.Participants.Select(p => p.Id).ToHashSet();

            var participantsToRemove = eventEntity.Participants
                .Where(p => !domainCollectionIds.Contains(p.Id))
                .ToList();

            foreach (var participant in participantsToRemove)
            {
                eventEntity.Participants.Remove(participant);
            }

            foreach (var participant in eventDomain.Participants)
            {
                if (!eventEntity.Participants.Any(p => p.Id == participant.Id))
                {
                    var participantEntity = _mapper.Map<ParticipantDb>(participant);
                    _dbContext.Attach(participantEntity);
                    eventEntity.Participants.Add(participantEntity);
                }
            }
        }



        public async Task SaveAsync(CancellationToken cancellationToken) 
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
