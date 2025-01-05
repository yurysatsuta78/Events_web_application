using Application.UseCases.Events.Update.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Events.Update
{
    public interface IUpdateEventInputPort : IInputPort
    {
        Task Handle(UpdateEventRequest request, CancellationToken cancellationToken);
    }
}
