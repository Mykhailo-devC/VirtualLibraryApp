using Microsoft.Extensions.Logging;
using System.Reflection;
using VirtualLibrary.Repository.Interface;

namespace VirtualLibrary.Logic.Implementation
{
    public class BookLogic : ModelLogicBase<BookCopy, BookDTO>
    {
        public BookLogic(IRepository<BookCopy, BookDTO> repository, ILogger<ModelLogicBase<BookCopy, BookDTO>> logger) : base(repository, logger)
        {
        }

        private const string ISBN = "ISBN";
        private const string AUTHOR = "Author";
        private const string NAME = "Name";
        private const string DATE = "Date";
        private const string PUBLISHER = "Publisher";

        public override async Task<ActionManagerResponse> GetDataAsync()
        {
            try
            {
                var books = await _repository.GetAllAsync();

                return new ActionManagerResponse<IEnumerable<BookCopy>>
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

                if (book == null)
                {
                    return new ActionManagerResponse
                    {
                        Success = false,
                        Message = "Data read error",
                        Errors = new List<string> { $"Incorrect id [Value = {id}]" }
                    };
                }

                return new ActionManagerResponse<BookCopy>
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

                if(!_repository.CheckModelField(modelField))
                {
                    return new ActionManagerResponse
                    {
                        Success = false,
                        Message = "Data read error",
                        Errors = new List<string> { $"Incorrect model field [Value = {modelField}]" }
                    };
                }

                var orderedBooks = books.OrderBy(b =>
                {
                    switch (modelField)
                    {
                        case ISBN: return b.Isbn.ToString();
                        case NAME: return b.Book.Name;
                        case AUTHOR: return b.Book.Author;
                        case DATE: return b.Item.PublishDate.ToString();
                        case PUBLISHER: return b.Item.Publisher.Name;
                        default: return b.CopyId.ToString();
                    }
                }).ToList();

                return new ActionManagerResponse<IEnumerable<BookCopy>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    ActionResult = orderedBooks
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'BookCopy' data");

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

            return new ActionManagerResponse<BookCopy>
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

            return new ActionManagerResponse<BookCopy>
            {
                Success = true,
                Message = "Data was read successfully",
                ActionResult = deletedBook
            };
        }
    }
}
