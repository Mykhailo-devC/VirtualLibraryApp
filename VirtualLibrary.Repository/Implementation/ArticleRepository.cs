﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace VirtualLibrary.Repository.Implementation
{
    public class ArticleRepository : RepositoryBase<ArticleCopy, ArticleDTO>
    {
        public ArticleRepository(VirtualLibraryDbContext context, ILogger<RepositoryBase<ArticleCopy, ArticleDTO>> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<ArticleCopy>> GetAllAsync()
        {
            var articles = _context.ArticleCopies
                    .Include(a => a.Article)
                    .Include(a => a.Item)
                        .ThenInclude(a => a.Publisher);

            foreach(var article in articles)
            {
                article.Article.ArticleCopies.Clear();
                article.Item.Publisher.Items.Clear();
            }

            _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                    "'ArticleCopy' data was read successfully");

            return await articles.ToListAsync();
        }

        public override async Task<ArticleCopy> GetByIdAsync(int id)
        {
            try
            {
                var article = await _context.ArticleCopies
                    .Include(a => a.Article)
                        .ThenInclude(a => a.MagazineArticles)
                    .Include(a => a.Item)
                        .ThenInclude(a => a.Publisher)
                    .FirstOrDefaultAsync(a => a.CopyId == id);

                if (article == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'ArticleCopy' data with id {id} was't found");
                    return null;
                }

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'ArticleCopy' data read successfully");

                return article;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"Failed read 'ArticleCopy' data [Id = {id}]");
                return null;
            }
        }
        public override async Task<ArticleCopy> CreateAsync(ArticleDTO articleDto)
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
                        "'ArticleCopy' data created successfully");

                    return articleCopy;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            "Failed adding 'ArticleCopy' data");

                    transaction.Rollback();

                    return null;
                }
            }
        }

        public override Task<ArticleCopy> UpdateAsync(int id, ArticleDTO entityDto)
        {
            throw new NotImplementedException();
        }

        public override async Task<ArticleCopy> DeleteAsync(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var articleCopy = await _context.ArticleCopies
                          .Include(a => a.Article)
                          .Include(a => a.Item)
                          .FirstOrDefaultAsync(a => a.CopyId == id);

                    if (articleCopy == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'ArticleCopy' data with id {id} was't found");
                        return null;
                    }

                    _context.Items.Remove(articleCopy.Item);
                    await SaveAsync();

                    if(!articleCopy.Article.ArticleCopies.Any())
                    {
                        _context.Articles.Remove(articleCopy.Article);
                        await SaveAsync();
                    }

                    await transaction.CommitAsync();
                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'ArticleCopy' data with id {articleCopy.CopyId} was removed successfully");

                    return articleCopy;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            $"Failed deleting 'ArticleCopy' data [Id = {id}]");

                    transaction.Rollback();

                    return null;
                }
            }
        }

        public override bool CheckModelField(string field)
        {
            if (string.IsNullOrWhiteSpace(field))
            {
                _logger.LogWarning($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            $"Model field is not valid [Field = {field}]");
                return false;
            }

            var listOfNames = new List<string>();

            listOfNames.AddRange(GetPropertyNames<ArticleCopy>());
            listOfNames.AddRange(GetPropertyNames<Article>());
            listOfNames.AddRange(GetPropertyNames<Item>());
            listOfNames.AddRange(GetPropertyNames<Publisher>());

            if (listOfNames.Contains(field))
            {
                return true;
            }
            else return false;
        }
    }
}
