using System.Reflection;
using VirtualLibrary.Utilites.Implementations.Filters.ModelFields;

namespace VirtualLibrary.Utilites.Implementations.DataStore
{
    public class MagazineDataStore : DataStoreBase<Magazine, MagazineDTO>
    {
        public MagazineDataStore(RepositoryFactory factory, ILogger<DataStoreBase<Magazine, MagazineDTO>> logger) : base(factory, logger)
        {
        }

        public override async Task<ActionManagerResponse<IEnumerable<Magazine>>> GetDataAsync()
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

                return new ActionManagerResponse<IEnumerable<Magazine>>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse<IEnumerable<Magazine>>> GetSortedDataAsync(ModelFields modelField)
        {
            try
            {
                var magazines = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Magazine>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = magazines.OrderBy(FieldParser.MagazineFields[modelField])
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Magazine' data");

                return new ActionManagerResponse<IEnumerable<Magazine>>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse<Magazine>> AddDataAsync(MagazineDTO entityDTO)
        {
            var newMagazine = await _repository.CreateAsync(entityDTO);

            if (newMagazine == null)
            {
                return new ActionManagerResponse<Magazine>
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

        public override async Task<ActionManagerResponse<Magazine>> DeleteDataAsync(int id)
        {
            var deletedMagazine = await _repository.DeleteAsync(id);

            if (deletedMagazine == null)
            {
                return new ActionManagerResponse<Magazine>
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

        

        public override Task<ActionManagerResponse<Magazine>> UpdateDataAsync(int id, MagazineDTO entityDTO)
        {
            throw new NotImplementedException();
        }
    }
}
