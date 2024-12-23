using Application.DTO.Request.Event;
using Application.DTO.Response.Event;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsContoller : ControllerBase
    {
        private readonly EventsService _eventsService;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;
        private readonly string _staticFilesPath =
            Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images");

        public EventsContoller(EventsService eventsService, ImageService imageService, IMapper mapper)
        {
            _eventsService = eventsService;
            _imageService = imageService;
            _mapper = mapper;
        }


        [HttpPost("create")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> CreateEvent([FromForm] CreateEventDto request,
            CancellationToken cancellationToken) 
        {
            var images = await _imageService.LoadImage(request.Images, _staticFilesPath,
                cancellationToken);

            var eventDomain = Event.Create(Guid.NewGuid(), request.Name, request.Description,
                request.EventTime, request.Location, request.Category, request.MaxParticipants);

            eventDomain.AddImages(images);

            await _eventsService.CreateEvent(eventDomain, cancellationToken);
            return Ok("Event created.");
        }


        [HttpPost("addparticipant/{eventId}")]
        [Authorize(Policy = "RequireParticipantRole")]
        public async Task<ActionResult> AddParticipantToIvent(Guid eventId, CancellationToken cancellationToken) 
        {
            var participantId = GetParticipantIdFromCookies();

            await _eventsService.AddParticipant(eventId, Guid.Parse(participantId), cancellationToken);

            return Ok("You are registered for the event.");
        }


        [HttpPost("removeparticipant/{eventId}")]
        [Authorize(Policy = "RequireParticipantRole")]
        public async Task<ActionResult> RemoveParticipantFromEvent(Guid eventId, CancellationToken cancellationToken) 
        {
            var participantId = GetParticipantIdFromCookies();

            await _eventsService.RemoveParticipant(eventId, Guid.Parse(participantId), cancellationToken);

            return Ok("You are removed from the event.");
        }


        [HttpGet("all")]
        public async Task<ActionResult> GetAllEvents(CancellationToken cancellationToken) 
        {
            return Ok(await _eventsService.GetAllEvents(cancellationToken));
        }


        [HttpGet("byid/{eventId}")]
        public async Task<ActionResult> GetById(Guid eventId, CancellationToken cancellationToken) 
        {
            return Ok(_mapper.Map<GetEventDto>(await _eventsService
                .GetEventById(eventId, cancellationToken)));
        }


        [HttpGet("byname/{eventName}")]
        public async Task<ActionResult> GetByName(string eventName, CancellationToken cancellationToken) 
        {
            return Ok(_mapper.Map<GetEventDto>(await _eventsService
                .GetEventByName(eventName, cancellationToken)));
        }


        [HttpGet("eventparticipants/{eventId}")]
        public async Task<ActionResult> GetEventParticipants(Guid eventId, CancellationToken cancellationToken) 
        {
            return Ok(_mapper.Map<GetEventParticipantsDto>(await _eventsService
                .GetEventById(eventId, cancellationToken)));
        }


        [HttpPut("update")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> UpdateEvent([FromForm] UpdateEventDto request,
            CancellationToken cancellationToken)
        {
            var eventDomain = await _eventsService.GetEventById(request.EventId, cancellationToken);

            eventDomain.UpdateEvent(request.Name, request.Description, request.EventTime, 
                request.Location, request.Category, request.MaxParticipants);

            await _eventsService.UpdateEvent(eventDomain, cancellationToken);
            return Ok("Event updated.");
        }


        [HttpDelete("remove/{eventId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteEvent(Guid eventId, CancellationToken cancellationToken) 
        {
            await _eventsService.DeleteEvent(eventId, cancellationToken);
            return Ok("Event removed.");
        }


        private string GetParticipantIdFromCookies() 
        {
            var participantIdClaim = User.Claims.FirstOrDefault(c => c.Type == "participantId");
            var participantId = participantIdClaim?.Value;

            if (participantId == null)
            {
                throw new InvalidOperationException("Participant ID is missing in the token.");
            }

            return participantId;
        }
    }
}
