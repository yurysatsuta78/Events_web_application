namespace Application.UseCases.Participants.Get.DTOs
{
    public class GetParticipantResponce
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
