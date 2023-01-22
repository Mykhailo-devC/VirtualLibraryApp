using System.Reflection;
using VirtualLibrary.Utilites.Implementations.Filters.ModelFields;

namespace VirtualLibrary.Utilites.Implementations.DataStore
{
    public class BookDataStore : DataStoreBase<Book, BookDTO>
    {
        public BookDataStore(RepositoryFactory factory, ILogger<DataStoreBase<Book, BookDTO>> logger) : base(factory, logger)
        {
        }

        public override async Task<ActionManagerResponse<IEnumerable<Book>>> GetDataAsync()
        {
            try
            {
                var books = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Book>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = books
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Book' data");

                return new ActionManagerResponse<IEnumerable<Book>>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse<IEnumerable<Book>>> GetSortedDataAsync(ModelFields modelField)
        {
            try
            {
                var books = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<Book>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = books.OrderBy(FieldParser.BookFields[modelField])
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Book' data");

                return new ActionManagerResponse<IEnumerable<Book>>
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse<Book>> AddDataAsync(BookDTO entityDTO)
        {
            var newBook = await _repository.CreateAsync(entityDTO);

            if(newBook == null) 
            {
                return new ActionManagerResponse<Book>
                {
                    Success = false,
                    Message = "Data creating transaction was interrapted",
                };
            }

            return new ActionManagerResponse<Book>
            {
                Success = true,
                Message = "Data was read successfully",
                ActionResult = newBook
            };
        }

        public override Task<ActionManagerResponse<Book>> UpdateDataAsync(int id, BookDTO entityDTO)
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionManagerResponse<Book>> DeleteDataAsync(int id)
        {
            var deletedBook = await _repository.DeleteAsync(id);

            if (deletedBook == null)
            {
                return new ActionManagerResponse<Book>
                {
                    Success = false,
                    Message = "Data deleting transaction was interrapted",
                };
            }

            return new ActionManagerResponse<Book>
            {
                Success = true,
                Message = "Data was read successfully",
                ActionResult = deletedBook
            };
        }
    }
}
