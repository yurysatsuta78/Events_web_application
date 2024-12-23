namespace Application.DTO.Request.Participant;

public record CreateParticipantDto
(
    string Name,
    string Surname,
    DateTime Birthday,
    string Email,
    string Password
);
