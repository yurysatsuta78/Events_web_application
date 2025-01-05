
using Domain.Interfaces;

namespace Application.UseCases.Events.Update
{
    public interface IRemoveParticipantInputPort : IInputPort
    {
        Task Handle(Guid id, string? participantId, CancellationToken cancellationToken);
    }
}
