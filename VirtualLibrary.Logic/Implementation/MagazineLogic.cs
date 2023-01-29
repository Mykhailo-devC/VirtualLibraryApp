using Microsoft.Extensions.Logging;
using System.Reflection;
using VirtualLibrary.Repository.Interface;

namespace VirtualLibrary.Logic.Implementation
{
    public class MagazineLogic : ModelLogicBase<MagazineCopy, MagazineDTO>
    {
        public MagazineLogic(IRepository<MagazineCopy, MagazineDTO> repository, ILogger<ModelLogicBase<MagazineCopy, MagazineDTO>> logger) : base(repository, logger)
        {
        }

        private const string ISSUE_NUMBER = "IssureNumber";
        private const string NAME = "Name";
        private const string DATE = "Date";
        private const string PUBLISHER = "Publisher";

        public override async Task<ActionManagerResponse> GetDataAsync()
        {
            try
            {
                var magazines = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<MagazineCopy>>
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

                return new ActionManagerResponse<MagazineCopy>
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

                if (!_repository.CheckModelField(modelField))
                {
                    return new ActionManagerResponse
                    {
                        Success = false,
                        Message = "Data read error",
                        Errors = new List<string> { $"Incorrect model field [Value = {modelField}]" }
                    };
                }

                var orderedMagazines = magazines.OrderBy(b =>
                {
                    switch (modelField)
                    {
                        case ISSUE_NUMBER: return b.IssureNumber.ToString();
                        case NAME: return b.Magazine.Name;
                        case DATE: return b.Item.PublishDate.ToString();
                        case PUBLISHER: return b.Item.Publisher.Name;
                        default: return b.CopyId.ToString();
                    }
                }).ToList();

                return new ActionManagerResponse<IEnumerable<MagazineCopy>>
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

            return new ActionManagerResponse<MagazineCopy>
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

            return new ActionManagerResponse<MagazineCopy>
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
