using Application.Interfaces.Repositories;

namespace Application.Interfaces.UnitsOfWork
{
    public interface IEventParticipantUOW
    {
        IEventsRepository EventsRepository { get; }
        IParticipantsRepository ParticipantsRepository { get; }
        Task SaveAsync(CancellationToken cancellationToken);
    }
}