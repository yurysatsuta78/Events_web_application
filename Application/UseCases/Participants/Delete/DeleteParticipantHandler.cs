using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases.Participants.Delete
{
    public class DeleteParticipantHandler : IDeleteParticipantInputPort
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteParticipantHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Guid id, CancellationToken cancellationToken)
        {
            var participantDomain = await _unitOfWork.ParticipantsRepository
                .GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Participant not found.");

            _unitOfWork.ParticipantsRepository.Delete(participantDomain);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
