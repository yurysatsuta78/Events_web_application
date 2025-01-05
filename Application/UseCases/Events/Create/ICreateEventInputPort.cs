using Application.UseCases.Events.Create.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Events.Create
{
    public interface ICreateEventInputPort : IInputPort
    {
        Task Handle(CreateEventRequest request, CancellationToken cancellationToken);
    }
}
