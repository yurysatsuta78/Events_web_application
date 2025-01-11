using Application.UseCases.Events.Update.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Events.Update
{
    public interface IAddParticipantInputPort : IInputPort
    {
        Task Handle(AddEventParticipantRequest request, CancellationToken cancellationToken);
    }
}
