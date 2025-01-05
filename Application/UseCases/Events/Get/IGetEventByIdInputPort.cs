using Application.UseCases.Events.Get.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Events.Get
{
    public interface IGetEventByIdInputPort : IInputPort
    {
        Task<GetEventResponce> Handle(Guid id, CancellationToken cancellationToken);
    }
}
