namespace Infrastructure.Entities
{
    public class RoleDb
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<ParticipantDb> Participants { get; set; } = new List<ParticipantDb>();
    }
}
