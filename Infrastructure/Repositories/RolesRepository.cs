using Application.Interfaces.Repositories;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class RolesRepository : BaseRepository<Role, int>, IRolesRepository
    {
        public RolesRepository(EventsDbContext dbContext) : base(dbContext) { }
    }
}