
using Application.UseCases.Events.Update.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Events.Update
{
    public interface IRemoveParticipantInputPort : IInputPort
    {
        Task Handle(RemoveEventParticipantRequest request, CancellationToken cancellationToken);
    }
}
