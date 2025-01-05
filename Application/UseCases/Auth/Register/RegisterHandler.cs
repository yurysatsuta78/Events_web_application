using Application.Services;
using Application.UseCases.Auth.Register.DTOs;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace Application.UseCases.Auth.Register
{
    public class RegisterHandler : IRegisterInputPort
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) 
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var hashedPassword = _passwordHasher.Generate(request.Password);

            var participantDomain = Participant.Create(Guid.NewGuid(), request.Name, request.Surname,
                request.Birthday, request.Email, hashedPassword);

            var roleDomain = await _unitOfWork.RolesRepository.GetByIdAsync(2, cancellationToken)
                ?? throw new NotFoundException("Role not found");

            participantDomain.AddRole(roleDomain);

            _unitOfWork.ParticipantsRepository.Add(participantDomain);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
