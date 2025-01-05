using Application.UseCases.Events.Get.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Events.Get
{
    public interface IGetAllEventsInputPort : IInputPort
    {
        Task<IEnumerable<GetEventResponce>> Handle(CancellationToken cancellationToken);
    }
}
