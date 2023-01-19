using System.Reflection;
using VirtualLibrary.Utilites.Interfaces;

namespace VirtualLibrary.Utilites.Implementations.Repositories
{
    public abstract class RepositoryBase<T, K> : IRepository<T, K> where T : class
    {
        protected VirtualLibraryDbContext _context;
        protected ILogger<RepositoryFactory> _logger;
        public RepositoryBase(VirtualLibraryDbContext context, ILogger<RepositoryFactory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public abstract Task<ActionManagerResponse<IEnumerable<T>>> GetAllAsync();
        public abstract Task<ActionManagerResponse<T>> GetByIdAsync(int id);
        public abstract Task<ActionManagerResponse<T>> CreateAsync(K entityDto);
        public abstract Task<ActionManagerResponse<T>> UpdateAsync(int id, K entityDto);
        public abstract Task<ActionManagerResponse<T>> DeleteAsync(int id);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'{typeof(T).Name}' data was saved");
        }
    }
}