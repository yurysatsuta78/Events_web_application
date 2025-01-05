namespace Application.UseCases.Auth.Register.DTOs;

public record RegisterRequest(string Name, string Surname, DateTime Birthday, string Email, string Password);
