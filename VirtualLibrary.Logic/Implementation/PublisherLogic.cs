using Microsoft.Extensions.Logging;
using System.Reflection;
using VirtualLibrary.Repository.Interface;

namespace VirtualLibrary.Logic.Implementation
{
    public class PublisherLogic : ModelLogicBase<Publisher, PublisherDTO>
    {
        public PublisherLogic(IRepository<Publisher, PublisherDTO> repository, ILogger<ModelLogicBase<Publisher, PublisherDTO>> logger) : base(repository, logger)
        {
        }

        private const string NAME = "Publisher";
        public override async Task<ActionManagerResponse> GetDataAsync()
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

                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse> GetDatabyId(int id)
        {
            try
            {
                var publishers = await _repository.GetByIdAsync(id);

                return new ActionManagerResponse<Publisher>
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

                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse> GetSortedDataAsync(string modelField)
        {
            try
            {
                var publishers = await _repository.GetAllAsync();

                if(!_repository.CheckModelField(publishers.FirstOrDefault(), modelField))
                {
                    return new ActionManagerResponse
                    {
                        Success = false,
                        Message = "Data read error",
                        Errors = new List<string> { $"Incorrect model field [Value = {modelField}]" }
                    };
                }

                var orderedPublishers = publishers.OrderBy(p =>
                {
                    switch (modelField)
                    {
                        case NAME: return p.Name;
                        default: return p.Id.ToString();
                    }
                }).ToList();

                return new ActionManagerResponse<IEnumerable<Publisher>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = orderedPublishers
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Publisher' data");

                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse> AddDataAsync(PublisherDTO entityDTO)
        {
            var newPublisher = await _repository.CreateAsync(entityDTO);

            if (newPublisher == null)
            {
                return new ActionManagerResponse
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

        public override async Task<ActionManagerResponse> UpdateDataAsync(int id, PublisherDTO entityDTO)
        {
            var updatedPublisher = await _repository.UpdateAsync(id, entityDTO);

            if (updatedPublisher == null)
            {
                return new ActionManagerResponse
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

        public override async Task<ActionManagerResponse> DeleteDataAsync(int id)
        {
            var deletedPublisher = await _repository.DeleteAsync(id);

            if (deletedPublisher == null)
            {
                return new ActionManagerResponse
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
