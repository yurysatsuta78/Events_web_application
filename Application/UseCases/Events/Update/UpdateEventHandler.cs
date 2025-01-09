using Application.UseCases.Events.Update.DTOs;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases.Events.Update
{
    public class UpdateEventHandler : IUpdateEventInputPort
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEventHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateEventRequest request, CancellationToken cancellationToken)
        {
            var eventDomain = await _unitOfWork.EventsRepository
                .GetByIdWithIncludesAsync(request.EventId, cancellationToken)
                ?? throw new NotFoundException("Event not found.");

            eventDomain.UpdateEvent(request.Name, request.Description, request.EventTime, request.Location,
                request.Category, request.MaxParticipants);

            _unitOfWork.EventsRepository.Update(eventDomain);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
