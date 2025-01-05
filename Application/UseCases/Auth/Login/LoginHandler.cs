using Application.Services;
using Application.UseCases.Auth.Login.DTOs;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases.Auth.Login
{
    public class LoginHandler : ILoginInputPort
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LoginHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<LoginResponce> Handle(LoginRequest request, DateTime currentTime, 
            CancellationToken cancellationToken)
        {
            var participantDomain = await _unitOfWork.ParticipantsRepository
                .GetByEmailAsync(request.Email, cancellationToken)
                ?? throw new NotFoundException("Participant not found.");

            var result = _passwordHasher.Verify(request.Password, participantDomain.PasswordHash);

            if (result == false)
            {
                throw new BadRequestException("Wrong password.");
            }

            var jwtToken = _jwtProvider.GenerateToken(participantDomain, currentTime);
            var refreshToken = _jwtProvider.GenerateRefreshToken(participantDomain.Id, currentTime);

            var tokens = new LoginResponce
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
            };

            return tokens;
        }
    }
}
