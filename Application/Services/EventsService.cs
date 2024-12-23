using Application.DTO.Response.Event;
using Application.Interfaces.UnitsOfWork;
using Domain.Models;

namespace Application.Services
{
    public class EventsService
    {
        private readonly IEventParticipantUOW _unitOfWork;

        public EventsService(IEventParticipantUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateEvent(Event eventDomain, CancellationToken cancellationToken) 
        {
            await _unitOfWork.EventsRepository.AddEvent(eventDomain, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task AddParticipant(Guid eventId, Guid participantId, 
            CancellationToken cancellationToken) 
        {
            var participantDomain = await _unitOfWork.ParticipantsRepository
                .GetParticipantById(participantId, cancellationToken);
            var eventDomain = await _unitOfWork.EventsRepository
                .GetEventById(eventId, cancellationToken);

            eventDomain.AddParticipant(participantDomain);

            await _unitOfWork.EventsRepository.UpdateEventParticipants(eventDomain, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task RemoveParticipant(Guid eventId, Guid participantId, 
            CancellationToken cancellationToken)
        {
            var eventDomain = await _unitOfWork.EventsRepository.GetEventById(eventId, cancellationToken);

            eventDomain.RemoveParticipant(participantId);

            await _unitOfWork.EventsRepository.UpdateEventParticipants(eventDomain, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task<Event> GetEventById(Guid eventId, CancellationToken cancellationToken) 
        {
            return await _unitOfWork.EventsRepository.GetEventById(eventId, cancellationToken);
        }

        public async Task<IEnumerable<GetEventDto>> GetAllEvents(CancellationToken cancellationToken) 
        {
            return await _unitOfWork.EventsRepository.GetAllEvents(cancellationToken);
        }

        public async Task<Event> GetEventByName(string eventTitle, CancellationToken cancellationToken) 
        {
            return await _unitOfWork.EventsRepository.GetEventByName(eventTitle, cancellationToken);
        }

        public async Task DeleteEvent(Guid eventId, CancellationToken cancellationToken) 
        {
            await _unitOfWork.EventsRepository.DeleteEvent(eventId, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task UpdateEvent(Event eventDomain, CancellationToken cancellationToken) 
        {
            await _unitOfWork.EventsRepository.UpdateEvent(eventDomain, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
