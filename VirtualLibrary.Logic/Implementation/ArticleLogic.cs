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

        public override async Task<Response> GetDataAsync()
        {
            try
            {
                var articles = await _repository.GetAllAsync();

                return new Response<IEnumerable<ArticleCopy>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = articles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Article' data");

                return new Response
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<Response> GetDatabyId(int id)
        {
            try
            {
                var article = await _repository.GetByIdAsync(id);

                return new Response<ArticleCopy>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = article
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Article' data");

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
                var articles = await _repository.GetAllAsync();

                if (!_repository.CheckModelField(modelField))
                {
                    return new Response
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

                return new Response<IEnumerable<ArticleCopy>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = orderedArticles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Article' data");

                return new Response
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public override async Task<Response> AddDataAsync(ArticleDTO entityDTO)
        {
            var newArticle = await _repository.CreateAsync(entityDTO);

            if (newArticle == null)
            {
                return new Response
                {
                    Success = false,
                    Message = "Data creating transaction was interrapted",
                };
            }

            return new Response<ArticleCopy>
            {
                Success = true,
                Message = "Data was read successfully",
                Data = newArticle
            };
        }

        public override async Task<Response> DeleteDataAsync(int id)
        {
            var deletedArticle = await _repository.DeleteAsync(id);

            if (deletedArticle == null)
            {
                return new Response
                {
                    Success = false,
                    Message = "Data deleting transaction was interrapted",
                };
            }

            return new Response<ArticleCopy>
            {
                Success = true,
                Message = "Data was read successfully",
                Data = deletedArticle
            };
        }

        

        public override Task<Response> UpdateDataAsync(int id, ArticleDTO entityDTO)
        {
            throw new NotImplementedException();
        }
    }
}
