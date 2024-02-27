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

        public override async Task<Response> GetDataAsync()
        {
            try
            {
                var magazines = await _repository.GetAllAsync();

                return new Response<IEnumerable<MagazineCopy>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = magazines
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Magazine' data");

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
                var magazine = await _repository.GetByIdAsync(int.Parse(id));

                return new Response<MagazineCopy>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = magazine
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Magazine' data");

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
                var magazines = await _repository.GetAllAsync();

                if (!_repository.CheckModelField(modelField))
                {
                    return new Response
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

                return new Response<IEnumerable<MagazineCopy>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = magazines
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Magazine' data");

                return new Response
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<Response> AddDataAsync(MagazineDTO entityDTO)
        {
            var newMagazine = await _repository.CreateAsync(entityDTO);

            if (newMagazine == null)
            {
                return new Response
                {
                    Success = false,
                    Message = "Data creating transaction was interrapted",
                };
            }

            return new Response<MagazineCopy>
            {
                Success = true,
                Message = "Data was read successfully",
                Data = newMagazine
            };
        }

        public override async Task<Response> DeleteDataAsync(string id)
        {
            var deletedMagazine = await _repository.DeleteAsync(int.Parse(id));

            if (deletedMagazine == null)
            {
                return new Response
                {
                    Success = false,
                    Message = "Data deleting transaction was interrapted",
                };
            }

            return new Response<MagazineCopy>
            {
                Success = true,
                Message = "Data was read successfully",
                Data = deletedMagazine
            };
        }

        

        public override Task<Response> UpdateDataAsync(string id, MagazineDTO entityDTO)
        {
            throw new NotImplementedException();
        }
    }
}
