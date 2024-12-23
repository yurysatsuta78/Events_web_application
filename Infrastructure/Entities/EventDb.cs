namespace Infrastructure.Entities
{
    public class EventDb
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int MaxParticipants { get; set; }
        public ICollection<ImageDb> Images {  get; set; } = new List<ImageDb>();
        public ICollection<ParticipantDb> Participants { get; set; } = new List<ParticipantDb>();
    }
}
