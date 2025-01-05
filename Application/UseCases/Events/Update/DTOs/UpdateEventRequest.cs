namespace Application.UseCases.Events.Update.DTOs;

public record UpdateEventRequest(Guid EventId, string? Name, string? Description, string? Location,
    string? Category, int? MaxParticipants, DateTime? EventTime);

