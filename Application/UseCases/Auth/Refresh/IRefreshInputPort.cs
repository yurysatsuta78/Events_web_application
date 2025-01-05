using Domain.Interfaces;

namespace Application.UseCases.Auth.Refresh
{
    public interface IRefreshInputPort : IInputPort
    {
        Task<string> Handle(string refreshToken, DateTime currentTime, CancellationToken cancellationToken);
    }
}
