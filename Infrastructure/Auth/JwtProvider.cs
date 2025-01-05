using Application.Services;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Auth
{
    public class JwtProvider : IJwtProvider
    {
        private readonly AuthOptions _options;

        public JwtProvider(AuthOptions options)
        {
            _options = options;
        }

        public string GenerateToken(Participant participant, DateTime current)
        {
            var claims = new List<Claim>
            {
                new Claim("participantId", participant.Id.ToString())
            };

            foreach (var role in participant.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var access = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: current.AddMinutes(AuthOptions.ACCESS_LIFETIME),
                signingCredentials: new SigningCredentials(AuthOptions
                .GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(access);
        }

        public string GenerateRefreshToken(Guid participantId, DateTime current)
        {
            var claims = new List<Claim>
            {
                new Claim("participantId", participantId.ToString())
            };

            var refresh = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: current.AddDays(AuthOptions.REFRESH_LIFETIME),
                signingCredentials: new SigningCredentials(AuthOptions
                .GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(refresh);
        }

        public ClaimsPrincipal ValidateRefreshToken(string refreshToken) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters 
            {
                ValidateIssuer = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidateAudience = true,
                ValidAudience = AuthOptions.AUDIENCE,
                ValidateLifetime = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true
            }, out var validatedToken);

            return principal;
        }
    }
}
