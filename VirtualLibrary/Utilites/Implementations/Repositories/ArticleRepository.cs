using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace VirtualLibrary.Utilites.Implementations.Repositories
{
    public class ArticleRepository : RepositoryBase<Article, ArticleDTO>
    {
        public ArticleRepository(VirtualLibraryDbContext context, ILogger<RepositoryFactory> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Article>> GetAllAsync()
        {
            //TODO: bring including magazines tp GetById method
            var articles = await _context.Articles
                    .Include(a => a.MagazineArticles)
                    .ThenInclude(a => a.Magazine)
                    .Include(a => a.ArticleCopies)
                    .ThenInclude(a => a.Item)
                    .ThenInclude(a => a.Publisher)
                    .ToListAsync();

            _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                    "'Article' data was read successfully");

            return articles;
        }

        public override async Task<Article> GetByIdAsync(int id)
        {
            try
            {
                var article = await _context.Articles
                    .Include(a => a.MagazineArticles)
                    .ThenInclude(a => a.Magazine)
                    .Include(a => a.ArticleCopies)
                    .ThenInclude(a => a.Item)
                    .ThenInclude(a => a.Publisher)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (article == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Article' data with id {id} was't found");
                    return null;
                }

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'Article' data read successfully");

                return article;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"Failed read 'Article' data [Id = {id}]");
                return null;
            }
        }
        public override async Task<Article> CreateAsync(ArticleDTO articleDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var publisher = await _context.Publishers.FindAsync(articleDto.publisherId);

                    if (publisher == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'Publisher' data with id {articleDto.publisherId} was't found");
                        return null;
                    }

                    var item = new Item
                    {
                        PublisherId = publisher.Id,
                    };

                    _context.Items.Add(item);
                    await SaveAsync();

                    var article = await _context.Articles.FirstOrDefaultAsync(b =>
                        b.Name == articleDto.Name && b.Author == articleDto.Author);

                    if (article == null)
                    {
                        article = new Article
                        {
                            Author = articleDto.Author,
                            Name = articleDto.Name,
                        };

                        _context.Articles.Add(article);
                        await SaveAsync();
                    }

                    var articleCopy = new ArticleCopy
                    {
                        Version = articleDto.Version,
                        ArticleId = article.Id,
                        ItemId = item.Id
                    };

                    _context.ArticleCopies.Add(articleCopy);
                    await SaveAsync();

                    if(articleDto.MagazineId != -1)
                    {
                        var maagzineArticle = new MagazineArticle
                        {
                            MagazineId = articleDto.MagazineId,
                            ArticleId = article.Id,
                        };

                        _context.MagazineArticles.Add(maagzineArticle);
                        await SaveAsync();
                    }

                    await transaction.CommitAsync();

                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'Article' data created successfully");

                    return article;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            "Failed adding 'Article' data");

                    transaction.Rollback();

                    return null;
                }
            }
        }

        public override Task<Article> UpdateAsync(int id, ArticleDTO entityDto)
        {
            throw new NotImplementedException();
        }

        public override async Task<Article> DeleteAsync(int id)
        {
            // TODO delete MagazineArticle entry too
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var article = await _context.Articles
                        .Include(a => a.MagazineArticles)
                        .ThenInclude(a => a.Magazine)
                        .Include(a => a.ArticleCopies)
                        .ThenInclude(a => a.Item)
                        .ThenInclude(a => a.Publisher)
                        .FirstOrDefaultAsync(a => a.Id == id);

                    if (article == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'Article' data with id {id} was't found");
                        return null;
                    }

                    var articleCopy = article.ArticleCopies.FirstOrDefault();

                    if (articleCopy == null)
                    {
                        throw new Exception($"Article with id {article.Id} has no copies");
                    }

                    _context.Items.Remove(articleCopy.Item);
                    _context.ArticleCopies.Remove(articleCopy);
                    await SaveAsync();

                    if (!article.ArticleCopies.Any())
                    {
                        _context.Articles.Remove(article);
                        await SaveAsync();
                    }

                    await transaction.CommitAsync();
                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'ArticleCopy' data with id {articleCopy.CopyId} was removed successfully");

                    return article;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            $"Failed deleting 'Article' data [Id = {id}]");

                    transaction.Rollback();

                    return null;
                }
            }
        }
    }
}
