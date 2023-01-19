using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace VirtualLibrary.Utilites.Implementations.Repositories
{
    public class PublisherRepository : RepositoryBase<Publisher, PublisherDTO>
    {
        public PublisherRepository(VirtualLibraryDbContext context, ILogger<RepositoryFactory> logger) : base(context, logger)
        {
        }

        public override async Task<ActionManagerResponse<IEnumerable<Publisher>>> GetAllAsync()
        {
            try
            {
                var result = new ActionManagerResponse<IEnumerable<Publisher>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = await _context.Publishers.ToListAsync()
                };

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'Publisher' data was read successfully");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Publisher' data");
                return new ActionManagerResponse<IEnumerable<Publisher>>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public override async Task<ActionManagerResponse<Publisher>> GetByIdAsync(int id)
        {
            try
            {
                var publisher = await _context.Publishers.FindAsync(id);

                if (publisher == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was't found");
                    return new ActionManagerResponse<Publisher>
                    {
                        Success = false,
                        Message = $"Data read error. No entry with id {id}",
                        Errors = new List<string> { "Result == null" }

                    };
                }

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was read successfully");

                return new ActionManagerResponse<Publisher>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = publisher
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        $"Failed read 'Publisher' data [Id = {id}]");
                return new ActionManagerResponse<Publisher>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public override async Task<ActionManagerResponse<Publisher>> CreateAsync(PublisherDTO publisherDto)
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

                return new ActionManagerResponse<Publisher>
                {
                    Success = true,
                    Message = "Data was added successfully",
                    ActionResult = newPublisher
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed adding 'Publisher' data");
                return new ActionManagerResponse<Publisher>
                {
                    Success = false,
                    Message = "Data add error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public override async Task<ActionManagerResponse<Publisher>> UpdateAsync(int id, PublisherDTO publisherDto)
        {
            try
            {
                var publisher = await _context.Publishers.FindAsync(id);

                if (publisher == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was't found");
                    return new ActionManagerResponse<Publisher>
                    {
                        Success = false,
                        Message = $"Not Found. No entry with id {id}",
                        Errors = new List<string> { "Result == null" }
                    };
                }

                publisher.Name = publisherDto.Name;
                await SaveAsync();

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data updated successfully");

                return new ActionManagerResponse<Publisher>
                {
                    Success = true,
                    Message = "Data was updated successfully",
                    ActionResult = publisher
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"Failed updating 'Publisher' data [Id = {id}]");

                return new ActionManagerResponse<Publisher>
                {
                    Success = false,
                    Message = $"Instance wasn't updated",
                    Errors = new List<string> { "Result == null" }

                };
            }
        }
        public override async Task<ActionManagerResponse<Publisher>> DeleteAsync(int id)
        {
            try
            {
                var publisher = await _context.Publishers.FindAsync(id);

                if (publisher == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was't found");
                    return new ActionManagerResponse<Publisher>
                    {
                        Success = false,
                        Message = $"Not Found. No entry with id {id}",
                        Errors = new List<string> { "Result == null" }

                    };
                }

                _context.Publishers.Remove(publisher);
                await SaveAsync();

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Publisher' data with id {id} was removed successfully");
                return new ActionManagerResponse<Publisher>
                {
                    Success = true,
                    Message = "Data was deleted successfully",
                    ActionResult = publisher
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        $"Failed deleting 'Publisher' data [Id = {id}]");
                return new ActionManagerResponse<Publisher>
                {
                    Success = false,
                    Message = "Data delete error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
