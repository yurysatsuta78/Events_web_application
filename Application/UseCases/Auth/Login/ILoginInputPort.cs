using Application.UseCases.Auth.Login.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Auth.Login
{
    public interface ILoginInputPort : IInputPort
    {
        Task<LoginResponce> Handle(LoginRequest request, DateTime currentTime, CancellationToken cancellationToken);
    }
}
