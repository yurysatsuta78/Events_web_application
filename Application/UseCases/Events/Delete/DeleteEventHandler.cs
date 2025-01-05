using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases.Events.Delete
{
    public class DeleteEventHandler : IDeleteEventInputPort
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEventHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Guid id, CancellationToken cancellationToken)
        {
            var eventDomain = await _unitOfWork.EventsRepository
                .GetByIdWithIncludesAsync(id, cancellationToken)
                ?? throw new NotFoundException("Event not found.");

            _unitOfWork.EventsRepository.Delete(eventDomain);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
