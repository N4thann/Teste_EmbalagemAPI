using SrManoelLoja.SeedWork;
using System.Linq.Expressions;

namespace SrManoelLoja.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetAllQueryable();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task SaveUpdateAsync(T entity);
        Task SaveDeleteAsync(int id);
        Task SaveAddAsync(T entity);
    }
}
