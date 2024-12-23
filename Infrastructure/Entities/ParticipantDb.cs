namespace Infrastructure.Entities
{
    public class ParticipantDb
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<EventDb> Events { get; set; } = new List<EventDb>();
        public ICollection<RoleDb> Roles { get; set; } = new List<RoleDb>();
    }
}
