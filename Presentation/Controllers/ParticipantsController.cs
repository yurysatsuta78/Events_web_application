using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly ParticipantsService _participantsService;

        public ParticipantsController(ParticipantsService participantsService)
        {
            _participantsService = participantsService;
        }


        [HttpGet("all")]
        public async Task<ActionResult> GetAllParticipants(CancellationToken cancellationToken) 
        {
            return Ok(await _participantsService.GetAllParticipants(cancellationToken));
        }



        [HttpGet("byid/{id}")]
        public async Task<ActionResult> GetParticipantById(Guid id, CancellationToken cancellationToken) 
        {
            return Ok(await _participantsService.GetParticipantById(id, cancellationToken));
        }



        [HttpGet("byemail/{email}")]
        public async Task<ActionResult> GetParticipantByEmail(string email, CancellationToken cancellationToken) 
        {
            return Ok(await _participantsService.GetParticipantByEmail(email, cancellationToken));
        }



        [HttpDelete("remove/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> DeleteParticipant(Guid id, CancellationToken cancellationToken) 
        {
            await _participantsService.DeleteParticipant(id, cancellationToken);
            return Ok("Participant removed.");
        }



        [HttpGet("allroles")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> GetAllRoles(CancellationToken cancellationToken) 
        {
            return Ok(await _participantsService.GetAllRoles(cancellationToken));
        }
    }
}
