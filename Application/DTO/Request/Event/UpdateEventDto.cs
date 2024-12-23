namespace Application.DTO.Request.Event;

public record UpdateEventDto
(
    Guid EventId,
    string? Name,
    string? Description,
    string? Location,
    string? Category,
    int? MaxParticipants,
    DateTime? EventTime
);
