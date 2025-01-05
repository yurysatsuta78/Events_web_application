using Application.UseCases.Events.Get.DTOs;
using AutoMapper;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases.Events.Get
{
    public class GetEventByIdHandler : IGetEventByIdInputPort
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEventByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetEventResponce> Handle(Guid id, CancellationToken cancellationToken)
        {
            var eventDomain = await _unitOfWork.EventsRepository.GetByIdWithIncludesAsync(id, cancellationToken);

            if (eventDomain == null) 
            {
                throw new NotFoundException("Event not found.");
            }

            return _mapper.Map<GetEventResponce>(eventDomain);
        }
    }
}
