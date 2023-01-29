using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace VirtualLibrary.Repository.Implementation
{
    public class MagazineRepository : RepositoryBase<MagazineCopy, MagazineDTO>
    {
        public MagazineRepository(VirtualLibraryDbContext context, ILogger<RepositoryBase<MagazineCopy, MagazineDTO>> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<MagazineCopy>> GetAllAsync()
        {
            var magazines =  _context.MagazineCopies
                    .Include(a => a.Magazine)
                    .Include(a => a.Item)
                        .ThenInclude(a => a.Publisher);

            foreach(var magazine in magazines)
            {
                magazine.Magazine.MagazineCopies.Clear();
                magazine.Item.Publisher.Items.Clear();
            }

            _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                    "'MagazineCopy' data was read successfully");

            return await magazines.ToListAsync();
        }

        public override async Task<MagazineCopy> GetByIdAsync(int id)
        {
            try
            {
                var magazine = await _context.MagazineCopies
                    .Include(a => a.Magazine)
                        .ThenInclude(a => a.MagazineArticles)
                    .Include(a => a.Item)
                        .ThenInclude(a => a.Publisher)
                    .FirstOrDefaultAsync(a => a.CopyId == id);

                if (magazine == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'MagazineCopy' data with id {id} was't found");
                    return null;
                }

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'MagazinesCopy' data read successfully");

                return magazine;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"Failed read 'MagazineCopy' data [Id = {id}]");
                return null;
            }
        }

        public override async Task<MagazineCopy> CreateAsync(MagazineDTO magazineDto)
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
                        "'MagazineCopy' data created successfully");

                    return magazineCopy;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            "Failed adding 'MagazineCopy' data");

                    transaction.Rollback();

                    return null;
                }
            }
        }

        public override async Task<MagazineCopy> UpdateAsync(int id, MagazineDTO entityDto)
        {
            throw new NotImplementedException();
        }

        public override async Task<MagazineCopy> DeleteAsync(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var magazineCopy = await _context.MagazineCopies
                          .Include(a => a.Magazine)
                          .Include(a => a.Item)
                          .FirstOrDefaultAsync(a => a.CopyId == id);

                    if (magazineCopy == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'MagazineCopy' data with id {id} was't found");
                        return null;
                    }

                    _context.Items.Remove(magazineCopy.Item);
                    await SaveAsync();

                    if (!magazineCopy.Magazine.MagazineCopies.Any())
                    {
                        _context.Magazines.Remove(magazineCopy.Magazine);
                        await SaveAsync();
                    }

                    await transaction.CommitAsync();
                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'MagazineCopy' data with id {magazineCopy.CopyId} was removed successfully");

                    return magazineCopy;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            $"Failed deleting 'MagazineCopy' data [Id = {id}]");

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

            listOfNames.AddRange(GetPropertyNames<MagazineCopy>());
            listOfNames.AddRange(GetPropertyNames<Magazine>());
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
