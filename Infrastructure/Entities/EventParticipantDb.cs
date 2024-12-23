namespace Infrastructure.Entities
{
    public class EventParticipantDb
    {
        public Guid EventId { get; set; }
        public Guid ParticipantId { get; set; }
        public DateTime EventRegistrationDate { get; set; }
    }
}
