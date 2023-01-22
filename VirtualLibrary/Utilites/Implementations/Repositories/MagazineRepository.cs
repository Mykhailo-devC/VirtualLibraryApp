using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VirtualLibrary.Models;

namespace VirtualLibrary.Utilites.Implementations.Repositories
{
    public class MagazineRepository : RepositoryBase<Magazine, MagazineDTO>
    {
        public MagazineRepository(VirtualLibraryDbContext context, ILogger<RepositoryFactory> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Magazine>> GetAllAsync()
        {
            var magazines = await _context.Magazines
                    .Include(a => a.MagazineArticles)
                    .ThenInclude(a => a.Article)
                    .Include(a => a.MagazineCopies)
                    .ThenInclude(a => a.Item)
                    .ThenInclude(a => a.Publisher)
                    .ToListAsync();

            _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                    "'Magazine' data was read successfully");

            return magazines;
        }

        public override async Task<Magazine> GetByIdAsync(int id)
        {
            try
            {
                var magazine = await _context.Magazines
                    .Include(a => a.MagazineArticles)
                    .ThenInclude(a => a.Article)
                    .Include(a => a.MagazineCopies)
                    .ThenInclude(a => a.Item)
                    .ThenInclude(a => a.Publisher)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (magazine == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Magazines' data with id {id} was't found");
                    return null;
                }

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'Magazines' data read successfully");

                return magazine;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"Failed read 'Magazine' data [Id = {id}]");
                return null;
            }
        }

        public override async Task<Magazine> CreateAsync(MagazineDTO magazineDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var publisher = await _context.Publishers.FindAsync(magazineDto.publisherId);

                    if (publisher == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'Publisher' data with id {magazineDto.publisherId} was't found");
                        return null;
                    }

                    var item = new Item
                    {
                        PublisherId = publisher.Id,
                    };

                    _context.Items.Add(item);
                    await SaveAsync();

                    var magazine = await _context.Magazines.FirstOrDefaultAsync(b =>
                        b.Name == magazineDto.Name);

                    if (magazine == null)
                    {
                        magazine = new Magazine
                        {
                            Name = magazineDto.Name,
                        };

                        _context.Magazines.Add(magazine);
                        await SaveAsync();
                    }

                    var magazineCopy = new MagazineCopy
                    {
                        IssureNumber = magazineDto.IssueNumber,
                        MagazineId = magazine.Id,
                        ItemId = item.Id
                    };

                    _context.MagazineCopies.Add(magazineCopy);
                    await SaveAsync();

                    if (magazineDto.ArticleId != -1)
                    {
                        var maagzineArticle = new MagazineArticle
                        {
                            MagazineId = magazine.Id,
                            ArticleId = magazineDto.ArticleId,
                        };

                        _context.MagazineArticles.Add(maagzineArticle);
                        await SaveAsync();
                    }

                    await transaction.CommitAsync();

                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'Magazine' data created successfully");

                    return magazine;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            "Failed adding 'Magazine' data");

                    transaction.Rollback();

                    return null;
                }
            }
        }

        public override async Task<Magazine> UpdateAsync(int id, MagazineDTO entityDto)
        {
            throw new NotImplementedException();
        }

        public override async Task<Magazine> DeleteAsync(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var magazine = await _context.Magazines
                    .Include(a => a.MagazineArticles)
                    .ThenInclude(a => a.Article)
                    .Include(a => a.MagazineCopies)
                    .ThenInclude(a => a.Item)
                    .ThenInclude(a => a.Publisher)
                    .FirstOrDefaultAsync(a => a.Id == id);

                    if (magazine == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'Magazine' data with id {id} was't found");
                        return null;
                    }

                    var magazineCopy = magazine.MagazineCopies.FirstOrDefault();

                    if (magazineCopy == null)
                    {
                        throw new Exception($"Article with id {magazine.Id} has no copies");
                    }
                    _context.Items.Remove(magazineCopy.Item);
                    _context.MagazineCopies.Remove(magazineCopy);
                    await SaveAsync();

                    if (!magazine.MagazineCopies.Any())
                    {
                        _context.Magazines.Remove(magazine);
                        await SaveAsync();
                    }

                    await transaction.CommitAsync();
                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'ArticleCopy' data with id {magazineCopy.CopyId} was removed successfully");

                    return magazine;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            $"Failed deleting 'Magazine' data [Id = {id}]");

                    transaction.Rollback();
                    return null;
                }
            }
        }
    }
}
