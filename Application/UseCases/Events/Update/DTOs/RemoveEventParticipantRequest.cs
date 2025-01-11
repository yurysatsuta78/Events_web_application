namespace Application.UseCases.Events.Update.DTOs;

public record RemoveEventParticipantRequest(Guid EventId, string ParticipantId);
