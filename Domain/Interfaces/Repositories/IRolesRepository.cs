using Domain.Interfaces.Repositories;
using Domain.Models;

namespace Application.Interfaces.Repositories
{
    public interface IRolesRepository : IBaseRepository<Role, int>
    {
    }
}