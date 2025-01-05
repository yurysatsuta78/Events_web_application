using Microsoft.AspNetCore.Mvc;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Http;
using Application.UseCases.Auth.Register;
using Application.UseCases.Auth.Register.DTOs;
using Application.UseCases.Auth.Login;
using Application.UseCases.Auth.Login.DTOs;
using Microsoft.AspNetCore.Authorization;
using Application.UseCases.Auth.Refresh;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterInputPort _registerInputPort;
        private readonly ILoginInputPort _loginInputPort;
        private readonly IRefreshInputPort _refreshInputPort;

        public AuthController(IRegisterInputPort registerInputPort, ILoginInputPort loginInputPort,
            IRefreshInputPort refreshInputPort)
        {
            _registerInputPort = registerInputPort;
            _loginInputPort = loginInputPort;
            _refreshInputPort = refreshInputPort;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request,
            CancellationToken cancellationToken)
        {
            await _registerInputPort.Handle(request, cancellationToken);

            return StatusCode(StatusCodes.Status201Created);
        }



        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var currentTime = DateTime.UtcNow;
            var tokens = await _loginInputPort.Handle(request, currentTime, cancellationToken);

            TokensToCookies(tokens.RefreshToken, tokens.JwtToken, currentTime);

            return Ok();
        }



        [Authorize]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Ok();
        }



        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshAccessToken(CancellationToken cancellationToken)
        {
            Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

            var currentTime = DateTime.UtcNow;
            var jwtToken = await _refreshInputPort.Handle(refreshToken, currentTime, cancellationToken);

            TokensToCookies(jwtToken, currentTime);

            return Ok();
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


        private void TokensToCookies(string jwtToken, DateTime current)
        {
            Response.Cookies.Append("accessToken", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = current.AddMinutes(AuthOptions.ACCESS_LIFETIME)
            });
        }
    }
}
