using VirtualLibrary.Utilites.Implementations.Repositories;
using VirtualLibrary.Utilites.Interfaces;

namespace VirtualLibrary.Utilites.Implementations
{
    public class RepositoryFactory
    {
        private readonly VirtualLibraryDbContext _context;
        private readonly ILogger<RepositoryFactory> _logger;

        public RepositoryFactory(VirtualLibraryDbContext context, ILogger<RepositoryFactory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IRepository<T, K> GetRepository<T, K>() where T : class
        {
            var genericType = typeof(T);

            if (genericType == typeof(Article))
            {
                //var repo = new ArticleRepository(_context, _logger);
                return null;
            }
            else if (genericType == typeof(Publisher))
            {
                var repo = new PublisherRepository(_context, _logger);
                return (IRepository<T, K>)repo;
            }
            return null;
        }
    }
}
