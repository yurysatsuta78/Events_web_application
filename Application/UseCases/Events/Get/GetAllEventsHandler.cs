using Application.UseCases.Events.Get.DTOs;
using AutoMapper;
using Domain.Interfaces;

namespace Application.UseCases.Events.Get
{
    public class GetAllEventsHandler : IGetAllEventsInputPort
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEventsHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetEventResponce>> Handle(CancellationToken cancellationToken)
        {
            var events = await _unitOfWork.EventsRepository.GetAllWithIncludesAsync(cancellationToken);

            return _mapper.Map<IEnumerable<GetEventResponce>>(events);
        }
    }
}
