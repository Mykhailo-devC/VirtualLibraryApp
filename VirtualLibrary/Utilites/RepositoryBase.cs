using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Utilites
{
    public interface IRepository<T, K>
    {
        public Task<ActionManagerResponse<IEnumerable<T>>> GetAllAsync();
        public Task<ActionManagerResponse<T>> GetByIdAsync(int id);
        public Task<ActionManagerResponse<T>> CreateAsync(K entity);
        public Task<ActionManagerResponse<T>> UpdateAsync(int id, K entity);
        public Task<ActionManagerResponse<T>> DeleteAsync(int id);
        public Task SaveAsync();
    }

    public class Repository : Repository<Article, ArticleDTO>
    {
        public Repository(VirtualLibraryDbContext context, ILogger<RepositoryFactory> logger) : base(context, logger)
        {
        }

        public override async  Task<ActionManagerResponse<Article>> CreateAsync(ArticleDTO entity)
        {
            throw new NotImplementedException();
        }
    }
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

            if(genericType == typeof(Article))
            {
                var repo = new Repository(_context, _logger);
                return (IRepository<T, K>)repo;
            }
            else
            {
                var repo = new Repository<T, K>(_context, _logger);
                return repo;
            }
        }
    }
    public class Repository<T, K> : IRepository<T, K> where T : class
    {
        protected VirtualLibraryDbContext _context;
        protected ILogger<RepositoryFactory> _logger;
        protected DbSet<T> _entity;
        public Repository(VirtualLibraryDbContext context, ILogger<RepositoryFactory> logger)
        {
            _context = context;
            _logger = logger;
            _entity = context.Set<T>();
        }
        public async Task<ActionManagerResponse<IEnumerable<T>>> GetAllAsync()
        {
            try
            {
                var result = new ActionManagerResponse<IEnumerable<T>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = await _entity.ToListAsync()
                };

                return result;
            }
            catch (Exception ex)
            {
                return new ActionManagerResponse<IEnumerable<T>>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<ActionManagerResponse<T>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _entity.FindAsync(id);

                if (result == null)
                {
                    return new ActionManagerResponse<T>
                    {
                        Success = false,
                        Message = $"Data read error. No entry with id {id}",
                        Errors = new List<string> { "Result == null" }

                    };
                }

                return new ActionManagerResponse<T>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = result
                };
            }
            catch (Exception ex)
            {
                return new ActionManagerResponse<T>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public virtual async Task<ActionManagerResponse<T>> CreateAsync(K entity)
        {
            try
            {
                var result = _context.CreateEntityInstance(entity);

                if(result is T newInstence)
                {
                    _entity.Add(newInstence);
                    await SaveAsync();

                    return new ActionManagerResponse<T>
                    {
                        Success = true,
                        Message = "Data was added successfully",
                        ActionResult = newInstence
                    };
                }

                return new ActionManagerResponse<T>
                {
                    Success = false,
                    Message = $"Instance wasn't created",
                    Errors = new List<string> { "Result == null" }

                };

            }
            catch (Exception ex)
            {
                return new ActionManagerResponse<T>
                {
                    Success = false,
                    Message = "Data add error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<ActionManagerResponse<T>> UpdateAsync(int id, K entity)
        {
            try
            {
                var result = await _entity.FindAsync(id);

                if (result == null)
                {
                    return new ActionManagerResponse<T>
                    {
                        Success = false,
                        Message = $"Not Found. No entry with id {id}",
                        Errors = new List<string> { "Result == null" }

                    };
                }

                _entity.Remove(result);
                await SaveAsync();

                return new ActionManagerResponse<T>
                {
                    Success = true,
                    Message = "Data was deleted successfully",
                    ActionResult = result
                };
            }
            catch (Exception ex)
            {
                return new ActionManagerResponse<T>
                {
                    Success = false,
                    Message = "Data delete error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<ActionManagerResponse<T>> DeleteAsync(int id)
        {
            try
            {
                var result = await _entity.FindAsync(id);

                if (result == null)
                {
                    return new ActionManagerResponse<T>
                    {
                        Success = false,
                        Message = $"Not Found. No entry with id {id}",
                        Errors = new List<string> { "Result == null" }

                    };
                }

                _entity.Remove(result);
                await SaveAsync();

                return new ActionManagerResponse<T>
                {
                    Success = true,
                    Message = "Data was deleted successfully",
                    ActionResult = result
                };
            }
            catch (Exception ex)
            {
                return new ActionManagerResponse<T>
                {
                    Success = false,
                    Message = "Data delete error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
