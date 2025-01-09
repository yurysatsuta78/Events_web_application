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

        public async Task Handle(Guid id, string? participantId, CancellationToken cancellationToken)
        {
            if (participantId == null)
            {
                throw new UnauthorizedException("Participant Id is missing.");
            }

            var participantDomain = await _unitOfWork.ParticipantsRepository
                .GetByIdAsync(Guid.Parse(participantId), cancellationToken)
                ?? throw new NotFoundException("Participant not found.");

            var eventDomain = await _unitOfWork.EventsRepository
                .GetByIdWithIncludesAsync(id, cancellationToken)
                ?? throw new NotFoundException("Event not found.");

            eventDomain.RemoveParticipant(participantDomain.Id);

            _unitOfWork.EventsRepository.Update(eventDomain);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
