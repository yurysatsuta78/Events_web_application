using Application.UseCases.Participants.Get.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Participants.Get
{
    public interface IGetAllParticipantsInputPort : IInputPort
    {
        Task<IEnumerable<GetParticipantResponce>> Handle(CancellationToken cancellationToken);
    }
}
