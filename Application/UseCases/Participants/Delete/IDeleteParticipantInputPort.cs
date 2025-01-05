using Domain.Interfaces;

namespace Application.UseCases.Participants.Delete
{
    public interface IDeleteParticipantInputPort : IInputPort
    {
        Task Handle(Guid id, CancellationToken cancellationToken);
    }
}
