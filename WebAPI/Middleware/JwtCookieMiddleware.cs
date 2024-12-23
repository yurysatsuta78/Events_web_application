using Infrastructure.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Middleware
{
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context) 
        {
            if (context.Request.Cookies.TryGetValue("accessToken", out var token)) 
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                }, out var validatedToken);

                context.User = principal;
            }

            await _next(context);
        }
    }
}
