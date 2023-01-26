global using VirtualLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VirtualLibrary.Repository.Interface;

namespace VirtualLibrary.Repository.Implementation
{
    public abstract class RepositoryBase<T, K> : IRepository<T, K> where T : class
    {
        protected VirtualLibraryDbContext _context;
        protected ILogger<RepositoryBase<T, K>> _logger;

        public RepositoryBase(VirtualLibraryDbContext context, ILogger<RepositoryBase<T, K>> logger)
        {
            _context = context;
            _logger = logger;
        }
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task<T> GetByIdAsync(int id);
        public abstract Task<T> CreateAsync(K entityDto);
        public abstract Task<T> UpdateAsync(int id, K entityDto);
        public abstract Task<T> DeleteAsync(int id);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public abstract bool CheckModelField(T entity, string field);

        protected List<string> GetPropertyNames<P>(P entity)
        {
            var result = new List<string>();
            var entityType = _context.Model.FindEntityType(typeof(P));
            foreach (var item in entityType.GetProperties().Select(e => e.GetColumnName())/*.Where(s => !s.Contains("Id"))*/.ToList())
            {
                result.Add(item);
            }

            return result;
        }
    }
}