using Application.UseCases.Auth.Register.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Auth.Register
{
    public interface IRegisterInputPort : IInputPort
    {
        Task Handle(RegisterRequest request, CancellationToken cancellationToken);
    }
}
