using Application.Services;
using Application;
using Microsoft.AspNetCore.Mvc;
using Application.DTO.Request.Participant;
using Domain.Models;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ParticipantsService _participantsService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(ParticipantsService participantsService, IPasswordHasher passwordHasher)
        {
            _participantsService = participantsService;
            _passwordHasher = passwordHasher;
        }


        [HttpPost("register")]
        public async Task<ActionResult> AddParticipant([FromBody] CreateParticipantDto request, 
            CancellationToken cancellationToken)
        {
            var hashedPassord = _passwordHasher.Generate(request.Password);

            var participantDomain = Participant.Create(Guid.NewGuid(), request.Name, request.Surname,
                request.Birthday, request.Email, hashedPassord);

            await _participantsService.AddParticipant(participantDomain, cancellationToken);

            return Ok("Participant created.");
        }



        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginParticipantDto request, 
            CancellationToken cancellationToken)
        {
            var current = DateTime.UtcNow;
            var tokens = await _participantsService.Login(request.Email, request.Password, 
                current, cancellationToken);

            TokensToCookies(tokens.RefreshToken, tokens.JwtToken, current);

            return Ok("Login successful.");
        }



        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshAccessToken(CancellationToken cancellationToken)
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized("No refresh token found.");
            }

            var current = DateTime.UtcNow;
            var tokens = await _participantsService.RefreshTokens(refreshToken, current, cancellationToken);

            TokensToCookies(tokens.RefreshToken, tokens.JwtToken, current);

            return Ok("Tokens updated successfully.");
        }



        [Authorize]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Ok("Logout succesfull.");
        }




        private void TokensToCookies(string refreshToken, string jwtToken, DateTime current)
        {
            Response.Cookies.Append("accessToken", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = current.AddMinutes(AuthOptions.ACCESS_LIFETIME)
            });

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = current.AddDays(AuthOptions.REFRESH_LIFETIME)
            });
        }
    }
}
