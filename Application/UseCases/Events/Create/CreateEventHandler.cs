using Application.Services;
using Application.UseCases.Events.Create.DTOs;
using Domain.Interfaces;
using Domain.Models;

namespace Application.UseCases.Events.Create
{
    public class CreateEventHandler : ICreateEventInputPort
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateEventHandler(IUnitOfWork unitOfWork,
            IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var images = await _imageService.LoadImages(request.Images, cancellationToken);

            var eventDomain = Event.Create(Guid.NewGuid(), request.Name, request.Description, request.EventTime,
                request.Location, request.Category, request.MaxParticipants);

            eventDomain.AddImages(images);

            await _unitOfWork.EventsRepository.AddAsync(eventDomain, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
