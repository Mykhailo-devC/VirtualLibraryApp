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
            var modelType = typeof(T);
            var dtoType = typeof(K);

            if (modelType == typeof(Book) && dtoType == typeof(BookDTO))
            {
                var repo = new BookRepository(_context, _logger);
                return (IRepository<T, K>)repo;
            }
            else if (modelType == typeof(Publisher) && dtoType == typeof(PublisherDTO))
            {
                var repo = new PublisherRepository(_context, _logger);
                return (IRepository<T, K>)repo;
            }
            else if (modelType == typeof(Article) && dtoType == typeof(ArticleDTO))
            {
                var repo = new ArticleRepository(_context, _logger);
                return (IRepository<T, K>)repo;
            }
            else if (modelType == typeof(Magazine) && dtoType == typeof(MagazineDTO))
            {
                var repo = new MagazineRepository(_context, _logger);
                return (IRepository<T, K>)repo;
            }

            throw new Exception($"Incorrect repository type, <{modelType.Name},{dtoType.Name}>");
        }
    }
}
