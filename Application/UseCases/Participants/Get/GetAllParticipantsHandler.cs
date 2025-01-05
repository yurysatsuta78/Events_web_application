using Application.UseCases.Participants.Get.DTOs;
using AutoMapper;
using Domain.Interfaces;

namespace Application.UseCases.Participants.Get
{
    public class GetAllParticipantsHandler : IGetAllParticipantsInputPort
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllParticipantsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetParticipantResponce>> Handle(CancellationToken cancellationToken)
        {
            var participants = await _unitOfWork.ParticipantsRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<GetParticipantResponce>>(participants);
        }
    }
}
