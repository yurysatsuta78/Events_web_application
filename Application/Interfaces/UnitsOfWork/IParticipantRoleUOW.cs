using Application.Interfaces.Repositories;

namespace Application.Interfaces.UnitsOfWork
{
    public interface IParticipantRoleUOW
    {
        IParticipantsRepository ParticipantsRepository { get; }
        IRolesRepository RolesRepository { get; }
        Task SaveAsync(CancellationToken cancellationToken);
    }
}