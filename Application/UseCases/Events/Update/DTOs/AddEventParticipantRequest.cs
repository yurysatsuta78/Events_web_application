namespace Application.UseCases.Events.Update.DTOs;

public record AddEventParticipantRequest(Guid EventId, string? ParticipantId);
