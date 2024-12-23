namespace Application.DTO.Response.Participant
{
    public class GetParticipantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> Events { get; set; } = new List<string>();
    }
}
