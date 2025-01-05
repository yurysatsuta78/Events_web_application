using Domain.Interfaces.Repositories;
using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IParticipantsRepository : IBaseRepository<Participant, Guid>
    {
        Task<Participant?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Participant?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken);
    }
}
