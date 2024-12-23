using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request.Event;

public record CreateEventDto
(
    string Name,
    string Description,
    DateTime EventTime,
    string Location,
    string Category,
    int MaxParticipants,
    [MaxLength(5)]IFormFile[] Images
);
