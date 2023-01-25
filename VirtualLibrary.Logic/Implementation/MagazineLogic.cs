using Microsoft.Extensions.Logging;
using System.Reflection;
using VirtualLibrary.Repository.Interface;

namespace VirtualLibrary.Logic.Implementation
{
    public class MagazineLogic : ModelLogicBase<Magazine, MagazineDTO>
    {
        public MagazineLogic(IRepository<Magazine, MagazineDTO> repository, ILogger<ModelLogicBase<Magazine, MagazineDTO>> logger) : base(repository, logger)
        {
        }

        public override async Task<ActionManagerResponse> GetDataAsync()
        {
            try
            {
                var magazines = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Magazine>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = magazines
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Magazine' data");

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
                var magazine = await _repository.GetByIdAsync(id);

                return new ActionManagerResponse<Magazine>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = magazine
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Magazine' data");

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
                var magazines = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Magazine>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = magazines
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Magazine' data");

                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse> AddDataAsync(MagazineDTO entityDTO)
        {
            var newMagazine = await _repository.CreateAsync(entityDTO);

            if (newMagazine == null)
            {
                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data creating transaction was interrapted",
                };
            }

            return new ActionManagerResponse<Magazine>
            {
                Success = true,
                Message = "Data was read successfully",
                ActionResult = newMagazine
            };
        }

        public override async Task<ActionManagerResponse> DeleteDataAsync(int id)
        {
            var deletedMagazine = await _repository.DeleteAsync(id);

            if (deletedMagazine == null)
            {
                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data deleting transaction was interrapted",
                };
            }

            return new ActionManagerResponse<Magazine>
            {
                Success = true,
                Message = "Data was read successfully",
                ActionResult = deletedMagazine
            };
        }

        

        public override Task<ActionManagerResponse> UpdateDataAsync(int id, MagazineDTO entityDTO)
        {
            throw new NotImplementedException();
        }
    }
}
