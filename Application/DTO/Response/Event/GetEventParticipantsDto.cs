namespace Application.DTO.Response.Event
{
    public class GetEventParticipantsDto
    {
        public string EventName { get; set; } = string.Empty;
        public IEnumerable<Guid> ParticipantIds { get; set; } = new List<Guid>();
    }
}
