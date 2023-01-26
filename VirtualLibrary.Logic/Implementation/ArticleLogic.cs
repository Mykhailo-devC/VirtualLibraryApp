using Microsoft.Extensions.Logging;
using System.Reflection;
using VirtualLibrary.Repository.Interface;

namespace VirtualLibrary.Logic.Implementation
{
    public class ArticleLogic : ModelLogicBase<ArticleCopy, ArticleDTO>
    {
        public ArticleLogic(IRepository<ArticleCopy, ArticleDTO> repository, ILogger<ModelLogicBase<ArticleCopy, ArticleDTO>> logger) : base(repository, logger)
        {
        }

        private const string VERSION = "Version";
        private const string AUTHOR = "Author";
        private const string NAME = "Name";
        private const string DATE = "Date";
        private const string PUBLISHER = "Publisher";

        public override async Task<ActionManagerResponse> GetDataAsync()
        {
            try
            {
                var articles = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<ArticleCopy>>
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

                return new ActionManagerResponse<ArticleCopy>
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

                if (!_repository.CheckModelField(articles.FirstOrDefault(), modelField))
                {
                    return new ActionManagerResponse
                    {
                        Success = false,
                        Message = "Data read error",
                        Errors = new List<string> { $"Incorrect model field [Value = {modelField}]" }
                    };
                }

                var orderedArticles = articles.OrderBy(b =>
                {
                    switch (modelField)
                    {
                        case VERSION: return b.Version.ToString();
                        case NAME: return b.Article.Name;
                        case AUTHOR: return b.Article.Author;
                        case DATE: return b.Item.PublishDate.ToString();
                        case PUBLISHER: return b.Item.Publisher.Name;
                        default: return b.CopyId.ToString();
                    }
                }).ToList();

                return new ActionManagerResponse<IEnumerable<ArticleCopy>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = orderedArticles
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

            return new ActionManagerResponse<ArticleCopy>
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

            return new ActionManagerResponse<ArticleCopy>
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
