using Microsoft.Extensions.Logging;
using System.Reflection;
using VirtualLibrary.Repository.Interface;

namespace VirtualLibrary.Logic.Implementation
{
    public class BookLogic : ModelLogicBase<Book, BookDTO>
    {
        public BookLogic(IRepository<Book, BookDTO> repository, ILogger<ModelLogicBase<Book, BookDTO>> logger) : base(repository, logger)
        {
        }

        public override async Task<ActionManagerResponse> GetDataAsync()
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
                var book = await _repository.GetByIdAsync(id);

                return new ActionManagerResponse<Book>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = book
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Book' data");

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

                return new ActionManagerResponse
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<ActionManagerResponse> AddDataAsync(BookDTO entityDTO)
        {
            var newBook = await _repository.CreateAsync(entityDTO);

            if(newBook == null) 
            {
                return new ActionManagerResponse
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

        public override Task<ActionManagerResponse> UpdateDataAsync(int id, BookDTO entityDTO)
        {
            throw new NotImplementedException();
        }

        public override async Task<ActionManagerResponse> DeleteDataAsync(int id)
        {
            var deletedBook = await _repository.DeleteAsync(id);

            if (deletedBook == null)
            {
                return new ActionManagerResponse
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
