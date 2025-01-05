using Application.Services;
using Application.UseCases.Events.Get;
using Application.UseCases.Events.Update;
using Application.UseCases.Participants.Delete;
using Application.UseCases.Participants.Get;
using Application.UseCases.Roles.Get;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IGetAllParticipantsInputPort _getAllParticipantsInputPort;
        private readonly IDeleteParticipantInputPort _deleteParticipantInputPort;
        private readonly IGetAllRolesInputPort _getAllRolesInputPort;

        public ParticipantsController(IGetAllParticipantsInputPort getAllParticipantsInputPort,
            IDeleteParticipantInputPort deleteParticipantInputPort, IGetAllRolesInputPort getAllRolesInputPort)
        {
            _getAllParticipantsInputPort = getAllParticipantsInputPort;
            _deleteParticipantInputPort = deleteParticipantInputPort;
            _getAllRolesInputPort = getAllRolesInputPort;
        }


        [HttpGet("all")]
        public async Task<ActionResult> GetAllParticipants(CancellationToken cancellationToken)
        {
            return Ok(await _getAllParticipantsInputPort.Handle(cancellationToken));
        }



        [HttpGet("allroles")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            return Ok(await _getAllRolesInputPort.Handle(cancellationToken));
        }



        [HttpDelete("remove/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteParticipant(Guid id, CancellationToken cancellationToken)
        {
            await _deleteParticipantInputPort.Handle(id, cancellationToken);

            return NoContent();
        }
    }
}
