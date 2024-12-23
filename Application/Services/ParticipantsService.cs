using Application.DTO;
using Application.DTO.Response.Participant;
using Application.Interfaces.Services;
using Application.Interfaces.UnitsOfWork;
using Domain.Exceptions;
using Domain.Models;

namespace Application.Services
{
    public class ParticipantsService
    {
        private readonly IParticipantRoleUOW _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public ParticipantsService(IParticipantRoleUOW unitOfWork, IPasswordHasher passwordHasher, IJwtProvider jwtProvider) 
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task AddParticipant(Participant participantDomain, CancellationToken cancellationToken) 
        {
            var roleDomain = await _unitOfWork.RolesRepository.GetRoleById(2, cancellationToken);

            participantDomain.AddRole(roleDomain);
            await _unitOfWork.ParticipantsRepository.AddParticipant(participantDomain, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);
        }

        public async Task<TokensDto> Login(string email, string password, DateTime current, CancellationToken cancellationToken) 
        {
            var participantDomain = await _unitOfWork.ParticipantsRepository.GetParticipantByEmail(email, cancellationToken);

            var result = _passwordHasher.Verify(password, participantDomain.PasswordHash);

            if (result == false) 
            {
                throw new InvalidPasswordException("Wrong password.");
            }

            var jwtToken = _jwtProvider.GenerateToken(participantDomain, current);
            var refreshToken = _jwtProvider.GenerateRefreshToken(participantDomain.Id, current);

            var tokens = new TokensDto
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
            };

            return tokens;
        }

        public async Task<TokensDto> RefreshTokens(string refreshToken, DateTime current, CancellationToken cancellationToken)
        {
            var principal = _jwtProvider.ValidateRefreshToken(refreshToken);
            var userIdClaim = principal.FindFirst("participantId");

            if (userIdClaim == null) 
            {
                throw new InvalidTokenException("Invalid refresh token.");
            }

            var participantDomain = await _unitOfWork.ParticipantsRepository.GetParticipantById(Guid.Parse(userIdClaim.Value), cancellationToken);

            var jwtToken = _jwtProvider.GenerateToken(participantDomain, current);
            var newRefreshToken = _jwtProvider.GenerateRefreshToken(participantDomain.Id, current);

            var tokens = new TokensDto
            {
                JwtToken = jwtToken,
                RefreshToken = newRefreshToken,
            };

            return tokens;
        }

        public async Task<IEnumerable<GetParticipantDto>> GetAllParticipants(CancellationToken cancellationToken) 
        {
            return await _unitOfWork.ParticipantsRepository.GetAllParticipants(cancellationToken);
        }

        public async Task<Participant> GetParticipantById(Guid participantId, CancellationToken cancellationToken) 
        {
            return await _unitOfWork.ParticipantsRepository.GetParticipantById(participantId, cancellationToken);
        }

        public async Task<Participant> GetParticipantByEmail(string email, CancellationToken cancellationToken) 
        {
            return await _unitOfWork.ParticipantsRepository.GetParticipantByEmail(email, cancellationToken);
        }

        public async Task DeleteParticipant(Guid id, CancellationToken cancellationToken) 
        {
            await _unitOfWork.ParticipantsRepository.DeleteParticipant(id, cancellationToken);
            await _unitOfWork.ParticipantsRepository.SaveAsync(cancellationToken);
        }

        public async Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken) 
        {
           return await _unitOfWork.RolesRepository.GetAllRoles(cancellationToken);
        }
    }
}
