namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T, TId> where T : class
    {
        Task AddAsync(T model, CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);  
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        void Update(T model);
        void Delete(T model);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
