using Microsoft.EntityFrameworkCore;
using SrManoelLoja.Data;
using SrManoelLoja.SeedWork;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SrManoelLoja.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context) => _context = context;

        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>()
                .AsNoTracking().ToListAsync();
        public IQueryable<T> GetAllQueryable() => _context.Set<T>().AsQueryable();
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) =>
            await _context.Set<T>().AnyAsync(predicate);

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }
        public async Task SaveUpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveDeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entidade com ID {id} não encontrada.");
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
