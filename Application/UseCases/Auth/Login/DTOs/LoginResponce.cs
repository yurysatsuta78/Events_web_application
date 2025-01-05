namespace Application.UseCases.Auth.Login.DTOs
{
    public class LoginResponce
    {
        public string JwtToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
