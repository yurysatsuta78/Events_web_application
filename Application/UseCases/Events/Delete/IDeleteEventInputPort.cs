using Domain.Interfaces;

namespace Application.UseCases.Events.Delete
{
    public interface IDeleteEventInputPort : IInputPort
    {
        Task Handle(Guid id, CancellationToken cancellationToken);
    }
}
