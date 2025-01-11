using Application.UseCases.Events.Update.DTOs;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases.Events.Update
{
    public class RemoveParticipantHandler : IRemoveParticipantInputPort
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveParticipantHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveEventParticipantRequest request, CancellationToken cancellationToken)
        {
            var participantDomain = await _unitOfWork.ParticipantsRepository
                .GetByIdAsync(Guid.Parse(request.ParticipantId), cancellationToken)
                ?? throw new NotFoundException("Participant not found.");

            var eventDomain = await _unitOfWork.EventsRepository
                .GetByIdWithIncludesAsync(request.EventId, cancellationToken)
                ?? throw new NotFoundException("Event not found.");

            eventDomain.RemoveParticipant(participantDomain.Id);

            _unitOfWork.EventsRepository.Update(eventDomain);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
