using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Events.Create.DTOs;

public record CreateEventRequest(string Name, string Description, DateTime EventTime, string Location,
    string Category, int MaxParticipants, [MaxLength(5)] IFormFile[] Images);
