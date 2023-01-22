using System.Reflection;
using VirtualLibrary.Utilites.Implementations.Filters.ModelFields;

namespace VirtualLibrary.Utilites.Implementations.DataStore
{
    public class PublisherDataStore : DataStoreBase<Publisher, PublisherDTO>
    {
        public PublisherDataStore(RepositoryFactory factory, ILogger<DataStoreBase<Publisher, PublisherDTO>> logger) : base(factory, logger)
        {
        }

        public override async Task<ActionManagerResponse<IEnumerable<Publisher>>> GetDataAsync()
        {
            try
            {
                var publishers = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Publisher>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = publishers
                };
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
        public override async Task<ActionManagerResponse<IEnumerable<Publisher>>> GetSortedDataAsync(ModelFields modelField)
        {
            try
            {
                var publishers = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Publisher>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = publishers.OrderBy(FieldParser.PublisherFields[modelField])
                };
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

        public override async Task<ActionManagerResponse<Publisher>> AddDataAsync(PublisherDTO entityDTO)
        {
            var newPublisher = await _repository.CreateAsync(entityDTO);

            if (newPublisher == null)
            {
                return new ActionManagerResponse<Publisher>
                {
                    Success = false,
                    Message = "Data creating transaction was interrapted",
                };
            }

            return new ActionManagerResponse<Publisher>
            {
                Success = true,
                Message = "Data was created successfully",
                ActionResult = newPublisher
            };
        }

        public override async Task<ActionManagerResponse<Publisher>> UpdateDataAsync(int id, PublisherDTO entityDTO)
        {
            var updatedPublisher = await _repository.UpdateAsync(id, entityDTO);

            if (updatedPublisher == null)
            {
                return new ActionManagerResponse<Publisher>
                {
                    Success = false,
                    Message = "Data updating transaction was interrapted",
                };
            }

            return new ActionManagerResponse<Publisher>
            {
                Success = true,
                Message = "Data has updated successfully",
                ActionResult = updatedPublisher
            };
        }

        public override async Task<ActionManagerResponse<Publisher>> DeleteDataAsync(int id)
        {
            var deletedPublisher = await _repository.DeleteAsync(id);

            if (deletedPublisher == null)
            {
                return new ActionManagerResponse<Publisher>
                {
                    Success = false,
                    Message = "Data deleting transaction was interrapted",
                };
            }

            return new ActionManagerResponse<Publisher>
            {
                Success = true,
                Message = "Data was deleted successfully",
                ActionResult = deletedPublisher
            };
        }
    }
}
