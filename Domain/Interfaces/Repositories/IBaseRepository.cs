namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T, TId> where T : class
    {
        void Add(T model);
        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);  
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        void Update(T model);
        void Delete(T model);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
