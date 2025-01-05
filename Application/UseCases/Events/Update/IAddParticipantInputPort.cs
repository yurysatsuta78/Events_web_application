using Domain.Interfaces;

namespace Application.UseCases.Events.Update
{
    public interface IAddParticipantInputPort : IInputPort
    {
        Task Handle(Guid id, string? participantId, CancellationToken cancellationToken);
    }
}
