using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace VirtualLibrary.Repository.Implementation
{
    public class PublisherRepository : RepositoryBase<Publisher, PublisherDTO>
    {
        public PublisherRepository(VirtualLibraryDbContext context, ILogger<RepositoryBase<Publisher, PublisherDTO>> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            var result = await _context.Publishers.ToListAsync();

            _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                    "'Publisher' data was read successfully");

            return result;
        }
        public override async Task<Publisher> GetByIdAsync(int id)
        {
            try
            {
                await _context.BookCopies.Include(b => b.Book).Where(b => b.Item.PublisherId == id).LoadAsync();
                await _context.ArticleCopies.Include(b => b.Article).Where(b => b.Item.PublisherId == id).LoadAsync();
                await _context.MagazineCopies.Include(b => b.Magazine).Where(b => b.Item.PublisherId == id).LoadAsync();

                var publisher = await _context.Publishers
                    .Include(p => p.Items)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (publisher == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was't found");
                    return null;
                }

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was read successfully");

                return publisher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        $"Failed read 'Publisher' data [Id = {id}]");
                return null;
            }
        }
        public override async Task<Publisher> CreateAsync(PublisherDTO publisherDto)
        {
            try
            {
                var newPublisher = new Publisher
                {
                    Name = publisherDto.Name,
                };

                await _context.Publishers.AddAsync(newPublisher);
                await SaveAsync();


                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                    "'Publisher' data created successfully");

                return newPublisher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed adding 'Publisher' data");

                return null;
            }
        }
        public override async Task<Publisher> UpdateAsync(int id, PublisherDTO publisherDto)
        {
            try
            {
                var publisher = await _context.Publishers.FindAsync(id);

                if (publisher == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was't found");

                    return null;
                }

                publisher.Name = publisherDto.Name;
                await SaveAsync();

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data updated successfully");

                return publisher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"Failed updating 'Publisher' data [Id = {id}]");

                return null;
            }
        }
        public override async Task<Publisher> DeleteAsync(int id)
        {
            //Tip: you can`' delete a publisher is he has published some items. 'Item' table can't exist without publisher TODO: enable Item`s nullable Publisher field
            try
            {
                var publisher = await _context.Publishers.FindAsync(id);

                if (publisher == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was't found");

                    return null;
                }

                _context.Publishers.Remove(publisher);
                await SaveAsync();

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was removed successfully");

                return publisher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        $"Failed deleting 'Publisher' data [Id = {id}]");

                return null;
            }
        }

        public override bool CheckModelField(Publisher entity, string field)
        {
            var listOfNames = new List<string>();

            listOfNames.AddRange(GetPropertyNames(entity));

            if (listOfNames.Contains(field))
            {
                return true;
            }
            else return false;
        }
    }
}
