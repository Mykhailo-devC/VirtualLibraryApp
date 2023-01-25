using Microsoft.Extensions.Logging;
using System.Reflection;
using VirtualLibrary.Repository.Interface;

namespace VirtualLibrary.Logic.Implementation
{
    public class ArticleLogic : ModelLogicBase<Article, ArticleDTO>
    {
        public ArticleLogic(IRepository<Article, ArticleDTO> repository, ILogger<ModelLogicBase<Article, ArticleDTO>> logger) : base(repository, logger)
        {
        }

        public override async Task<ActionManagerResponse> GetDataAsync()
        {
            try
            {
                var articles = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Article>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = articles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Article' data");

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
                var article = await _repository.GetByIdAsync(id);

                return new ActionManagerResponse<Article>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = article
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Article' data");

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
                var articles = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Article>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = articles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Article' data");

                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public override async Task<ActionManagerResponse> AddDataAsync(ArticleDTO entityDTO)
        {
            var newArticle = await _repository.CreateAsync(entityDTO);

            if (newArticle == null)
            {
                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data creating transaction was interrapted",
                };
            }

            return new ActionManagerResponse<Article>
            {
                Success = true,
                Message = "Data was read successfully",
                ActionResult = newArticle
            };
        }

        public override async Task<ActionManagerResponse> DeleteDataAsync(int id)
        {
            var deletedArticle = await _repository.DeleteAsync(id);

            if (deletedArticle == null)
            {
                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data deleting transaction was interrapted",
                };
            }

            return new ActionManagerResponse<Article>
            {
                Success = true,
                Message = "Data was read successfully",
                ActionResult = deletedArticle
            };
        }

        

        public override Task<ActionManagerResponse> UpdateDataAsync(int id, ArticleDTO entityDTO)
        {
            throw new NotImplementedException();
        }
    }
}
