namespace Application.DTO
{
    public class TokensDto
    {
        public string JwtToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
