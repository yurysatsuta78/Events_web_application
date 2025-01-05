using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T, TId> : IBaseRepository<T, TId> 
        where T : class, IModel<TId>
    {
        protected readonly EventsDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(EventsDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual void Add(T model)
        {
            _dbSet.Add(model);
        }

        public void Delete(T model)
        {
            _dbSet.Remove(model);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken)
        {
            var model = await _dbSet.FindAsync(id, cancellationToken);

            return model;
        }

        public void Update(T model)
        {
            _dbSet.Attach(model);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
