using Application.DTO.Response.Participant;
using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IParticipantsRepository
    {
        Task AddParticipant(Participant participantDomain, CancellationToken cancellationToken);
        Task<Participant> GetParticipantById(Guid id, CancellationToken cancellationToken);
        Task<Participant> GetParticipantByEmail(string email, CancellationToken cancellationToken);
        Task<IEnumerable<GetParticipantDto>> GetAllParticipants(CancellationToken cancellationToken);
        Task DeleteParticipant(Guid id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
