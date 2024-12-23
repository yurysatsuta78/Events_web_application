using Application.DTO.Response.Event;
using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IEventsRepository
    {
        Task AddEvent(Event eventDomain, CancellationToken cancellationToken);
        Task<Event> GetEventById(Guid id, CancellationToken cancellationToken);
        Task<Event> GetEventByName(string name, CancellationToken cancellationToken);
        Task<IEnumerable<GetEventDto>> GetAllEvents(CancellationToken cancellationToken);
        Task UpdateEvent(Event eventDomain, CancellationToken cancellationToken);
        Task DeleteEvent(Guid id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
        Task UpdateEventParticipants(Event eventDomain, CancellationToken cancellationToken);
    }
}