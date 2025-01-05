namespace Application.UseCases.Events.Get.DTOs;

public class GetEventResponce
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public int Participants { get; set; }
    public IEnumerable<string> ImagePaths { get; set; } = new List<string>();
}
