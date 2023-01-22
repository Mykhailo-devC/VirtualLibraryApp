using System.Reflection;
using VirtualLibrary.Utilites.Implementations.Filters.ModelFields;

namespace VirtualLibrary.Utilites.Implementations.DataStore
{
    public class ArticleDataStore : DataStoreBase<Article, ArticleDTO>
    {
        public ArticleDataStore(RepositoryFactory factory, ILogger<DataStoreBase<Article, ArticleDTO>> logger) : base(factory, logger)
        {
        }

        public override async Task<ActionManagerResponse<IEnumerable<Article>>> GetDataAsync()
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

                return new ActionManagerResponse<IEnumerable<Article>>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse<IEnumerable<Article>>> GetSortedDataAsync(ModelFields modelField)
        {
            try
            {
                var articles = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Article>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = articles.OrderBy(FieldParser.ArticleFields[modelField])
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Article' data");

                return new ActionManagerResponse<IEnumerable<Article>>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public override async Task<ActionManagerResponse<Article>> AddDataAsync(ArticleDTO entityDTO)
        {
            var newArticle = await _repository.CreateAsync(entityDTO);

            if (newArticle == null)
            {
                return new ActionManagerResponse<Article>
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

        public override async Task<ActionManagerResponse<Article>> DeleteDataAsync(int id)
        {
            var deletedArticle = await _repository.DeleteAsync(id);

            if (deletedArticle == null)
            {
                return new ActionManagerResponse<Article>
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

        

        public override Task<ActionManagerResponse<Article>> UpdateDataAsync(int id, ArticleDTO entityDTO)
        {
            throw new NotImplementedException();
        }
    }
}
