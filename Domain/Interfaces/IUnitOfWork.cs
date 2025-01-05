using Application.Interfaces.Repositories;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEventsRepository EventsRepository { get; }
        IParticipantsRepository ParticipantsRepository { get; }
        IRolesRepository RolesRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
