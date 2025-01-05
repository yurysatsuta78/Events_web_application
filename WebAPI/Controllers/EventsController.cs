using Application.UseCases.Events.Create;
using Application.UseCases.Events.Create.DTOs;
using Application.UseCases.Events.Delete;
using Application.UseCases.Events.Get;
using Application.UseCases.Events.Update;
using Application.UseCases.Events.Update.DTOs;
using Application.UseCases.Participants.Get;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {   
        private readonly ICreateEventInputPort _createEventInputPort;
        private readonly IDeleteEventInputPort _deleteEventInputPort;
        private readonly IUpdateEventInputPort _updateEventInputPort;
        private readonly IGetEventByIdInputPort _getEventByIdInputPort;
        private readonly IGetAllEventsInputPort _getAllEventsInputPort;
        private readonly IAddParticipantInputPort _addParticipantInputPort;
        private readonly IRemoveParticipantInputPort _removeParticipantInputPort;
        private readonly IGetEventParticipantsInputPort _getEventParticipantsInputPort;

        public EventsController(ICreateEventInputPort createEventInputPort, IDeleteEventInputPort deleteEventInputPort,
            IUpdateEventInputPort updateEventInputPort, IGetEventByIdInputPort getEventByIdInputPort,
            IGetAllEventsInputPort getAllEventsInputPort, IAddParticipantInputPort addParticipantInputPort,
            IRemoveParticipantInputPort removeParticipantInputPort, IGetEventParticipantsInputPort getEventParticipantsInputPort) 
        {
            _createEventInputPort = createEventInputPort;
            _deleteEventInputPort = deleteEventInputPort;
            _updateEventInputPort = updateEventInputPort;
            _getEventByIdInputPort = getEventByIdInputPort;
            _getAllEventsInputPort = getAllEventsInputPort;
            _addParticipantInputPort = addParticipantInputPort;
            _removeParticipantInputPort = removeParticipantInputPort;
            _getEventParticipantsInputPort = getEventParticipantsInputPort;
        }

        [HttpPost("create")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> Create([FromForm] CreateEventRequest request,
            CancellationToken cancellationToken)
        {
            await _createEventInputPort.Handle(request, cancellationToken);

            return StatusCode(StatusCodes.Status201Created);
        }



        [HttpGet("byid/{eventId}")]
        public async Task<ActionResult> GetById(Guid eventId, CancellationToken cancellationToken)
        {
            return Ok(await _getEventByIdInputPort.Handle(eventId, cancellationToken));
        }



        [HttpGet("all")]
        public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _getAllEventsInputPort.Handle(cancellationToken));
        }



        [HttpGet("eventparticipants/{eventId}")]
        public async Task<ActionResult> GetEventParticipants(Guid eventId, CancellationToken cancellationToken) 
        {
            return Ok(await _getEventParticipantsInputPort.Handle(eventId, cancellationToken));
        }



        [HttpPut("update")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> Update([FromForm] UpdateEventRequest request,
            CancellationToken cancellationToken)
        {
            await _updateEventInputPort.Handle(request, cancellationToken);

            return NoContent();
        }



        [HttpPut("addparticipant/{eventId}")]
        [Authorize(Policy = "RequireParticipantRole")]
        public async Task<ActionResult> AddParticipantToIvent(Guid eventId, CancellationToken cancellationToken)
        {
            var participantId = GetParticipantIdFromCookies();

            await _addParticipantInputPort.Handle(eventId, participantId, cancellationToken);

            return StatusCode(StatusCodes.Status200OK);
        }



        [HttpPut("removeparticipant/{eventId}")]
        [Authorize(Policy = "RequireParticipantRole")]
        public async Task<ActionResult> RemoveParticipantFromEvent(Guid eventId, CancellationToken cancellationToken)
        {
            var participantId = GetParticipantIdFromCookies();

            await _removeParticipantInputPort.Handle(eventId, participantId, cancellationToken);

            return StatusCode(StatusCodes.Status200OK);
        }



        [HttpDelete("delete/{eventId}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> Delete(Guid eventId, CancellationToken cancellationToken)
        {
            await _deleteEventInputPort.Handle(eventId, cancellationToken);

            return NoContent();
        }



        private string? GetParticipantIdFromCookies()
        {
            var participantIdClaim = User.Claims.FirstOrDefault(c => c.Type == "participantId");
            var participantId = participantIdClaim?.Value;

            return participantId;
        }
    }
}
