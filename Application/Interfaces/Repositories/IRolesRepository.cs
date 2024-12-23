using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IRolesRepository
    {
        Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken);
        Task<Role> GetRoleById(int id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}