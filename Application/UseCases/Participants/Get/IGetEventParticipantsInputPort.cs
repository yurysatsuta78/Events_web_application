using Application.UseCases.Participants.Get.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Participants.Get
{
    public interface IGetEventParticipantsInputPort : IInputPort
    {
        Task<IEnumerable<GetParticipantResponce>> Handle(Guid id, CancellationToken cancellationToken);
    }
}
