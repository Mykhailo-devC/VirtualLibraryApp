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
        public override async Task<Response> GetDataAsync()
        {
            try
            {
                var publishers = await _repository.GetAllAsync();

                return new Response<IEnumerable<Publisher>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = publishers
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Publisher' data");

                return new Response
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<Response> GetDatabyId(string id)
        {
            try
            {
                var publishers = await _repository.GetByIdAsync(int.Parse(id));

                return new Response<Publisher>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = publishers
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Publisher' data");

                return new Response
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<Response> GetSortedDataAsync(string modelField)
        {
            try
            {
                var publishers = await _repository.GetAllAsync();

                if(!_repository.CheckModelField(modelField))
                {
                    return new Response
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

                return new Response<IEnumerable<Publisher>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = orderedPublishers
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Publisher' data");

                return new Response
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<Response> AddDataAsync(PublisherDTO entityDTO)
        {
            var newPublisher = await _repository.CreateAsync(entityDTO);

            if (newPublisher == null)
            {
                return new Response
                {
                    Success = false,
                    Message = "Data creating transaction was interrapted",
                };
            }

            return new Response<Publisher>
            {
                Success = true,
                Message = "Data was created successfully",
                Data = newPublisher
            };
        }

        public override async Task<Response> UpdateDataAsync(string id, PublisherDTO entityDTO)
        {
            var updatedPublisher = await _repository.UpdateAsync(int.Parse(id), entityDTO);

            if (updatedPublisher == null)
            {
                return new Response
                {
                    Success = false,
                    Message = "Data updating transaction was interrapted",
                };
            }

            return new Response<Publisher>
            {
                Success = true,
                Message = "Data has updated successfully",
                Data = updatedPublisher
            };
        }

        public override async Task<Response> DeleteDataAsync(string id)
        {
            var deletedPublisher = await _repository.DeleteAsync(int.Parse(id));

            if (deletedPublisher == null)
            {
                return new Response
                {
                    Success = false,
                    Message = "Data deleting transaction was interrapted",
                };
            }

            return new Response<Publisher>
            {
                Success = true,
                Message = "Data was deleted successfully",
                Data = deletedPublisher
            };
        }

        
    }
}
