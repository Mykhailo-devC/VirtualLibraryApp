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

        public override async Task<Response> GetDataAsync()
        {
            try
            {
                var books = await _repository.GetAllAsync();

                return new Response<IEnumerable<BookCopy>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = books
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Book' data");

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
                var book = await _repository.GetByIdAsync(int.Parse(id));

                if (book == null)
                {
                    return new Response
                    {
                        Success = false,
                        Message = "Data read error",
                        Errors = new List<string> { $"Incorrect id [Value = {id}]" }
                    };
                }

                return new Response<BookCopy>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = book
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'Book' data");

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
                var books = await _repository.GetAllAsync();

                if(!_repository.CheckModelField(modelField))
                {
                    return new Response
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

                return new Response<IEnumerable<BookCopy>>
                {
                    Success = true,
                    Message = "Data was read successfully",
                    Data = orderedBooks
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                        "Failed read 'BookCopy' data");

                return new Response
                {
                    Success = false,
                    Message = "Data read error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public override async Task<Response> AddDataAsync(BookDTO entityDTO)
        {
            var newBook = await _repository.CreateAsync(entityDTO);

            if(newBook == null) 
            {
                return new Response
                {
                    Success = false,
                    Message = "Data creating transaction was interrapted",
                };
            }

            return new Response<BookCopy>
            {
                Success = true,
                Message = "Data was read successfully",
                Data = newBook
            };
        }

        public override Task<Response> UpdateDataAsync(string id, BookDTO entityDTO)
        {
            throw new NotImplementedException();
        }

        public override async Task<Response> DeleteDataAsync(string id)
        {
            var deletedBook = await _repository.DeleteAsync(int.Parse(id));

            if (deletedBook == null)
            {
                return new Response
                {
                    Success = false,
                    Message = "Data deleting transaction was interrapted",
                };
            }

            return new Response<BookCopy>
            {
                Success = true,
                Message = "Data was read successfully",
                Data = deletedBook
            };
        }
    }
}
