using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly EventsDbContext _dbContext;
        private readonly IMapper _mapper;

        public RolesRepository(EventsDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Role> GetRoleById(int id, CancellationToken cancellationToken)
        {
            var roleEntity = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (roleEntity == null)
            {
                throw new EntityNotFoundException($"{nameof(roleEntity)} not found.");
            }

            return _mapper.Map<Role>(roleEntity);
        }

        public async Task<IEnumerable<Role>> GetAllRoles(CancellationToken cancellationToken) 
        {
            var roleEntities = await _dbContext.Roles.AsNoTracking().ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<Role>>(roleEntities);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
