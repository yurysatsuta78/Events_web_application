
using Application.Services;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.UseCases.Auth.Refresh
{
    public class RefreshHandler : IRefreshInputPort
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshHandler(IJwtProvider jwtProvider, IUnitOfWork unitOfWork)
        {
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(string refreshToken, DateTime currentTime, 
            CancellationToken cancellationToken)
        {
            if (refreshToken == null) 
            {
                throw new UnauthorizedException("Refresh token not found.");
            }

            var principal = _jwtProvider.ValidateRefreshToken(refreshToken);
            var userIdClaim = principal.FindFirst("participantId")
                ?? throw new UnauthorizedException("Invalid refresh token.");

            var participantDomain = await _unitOfWork.ParticipantsRepository
                .GetByIdWithIncludesAsync(Guid.Parse(userIdClaim.Value), cancellationToken)
                ?? throw new NotFoundException("Participant not found.");

            var jwtToken = _jwtProvider.GenerateToken(participantDomain, currentTime);

            return jwtToken;
        }
    }
}
