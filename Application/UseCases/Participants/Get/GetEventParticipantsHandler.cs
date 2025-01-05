using Application.UseCases.Participants.Get.DTOs;
using AutoMapper;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases.Participants.Get
{
    public class GetEventParticipantsHandler : IGetEventParticipantsInputPort
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventParticipantsHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetParticipantResponce>> Handle(Guid id, CancellationToken cancellationToken)
        {
            var eventDomain = await _unitOfWork.EventsRepository
                .GetByIdWithIncludesAsync(id, cancellationToken)
                ?? throw new NotFoundException("Event not found.");

            return _mapper.Map<IEnumerable<GetParticipantResponce>>(eventDomain.Participants);
        }
    }
}
