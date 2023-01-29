using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using VirtualLibrary.Models;

namespace VirtualLibrary.Repository.Implementation
{
    public class BookRepository : RepositoryBase<BookCopy, BookDTO>
    {
        public BookRepository(VirtualLibraryDbContext context, ILogger<RepositoryBase<BookCopy, BookDTO>> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<BookCopy>> GetAllAsync()
        {
            var books = _context.BookCopies
                                .Include(e => e.Book)
                                .Include(e => e.Item)
                                    .ThenInclude(e => e.Publisher);

           foreach(var book in books)
            {
                book.Book.BookCopies.Clear();
                book.Item.Publisher.Items.Clear();
            };


            _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'BookCopy' data was read successfully");
            
            return await books.ToListAsync();
        }

        public override async Task<BookCopy> GetByIdAsync(int id)
        {
            try
            {
                var book = await _context.BookCopies
                    .Include(e => e.Book)
                    .Include(e => e.Item)
                    .ThenInclude(e => e.Publisher)
                    .FirstOrDefaultAsync(b => b.CopyId == id);

                if (book == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'BookCopy' data with id {id} was't found");
                    return null;
                }

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'BookCopy' data read successfully");

                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"Failed read 'BookCopy' data [Id = {id}]");
                return null;
            }
        }

        public override async Task<BookCopy> CreateAsync(BookDTO bookDto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var publisher = await _context.Publishers.FindAsync(bookDto.publisherId);

                    if (publisher == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'Publisher' data with id {bookDto.publisherId} was't found");
                        return null;
                    }

                    var item = new Item
                    {
                        PublisherId = publisher.Id,
                    };

                    _context.Items.Add(item);
                    await SaveAsync();

                    var book = await _context.Books.FirstOrDefaultAsync(b =>
                        b.Name == bookDto.Name && b.Author == bookDto.Author);

                    if (book == null)
                    {
                        book = new Book
                        {
                            Author = bookDto.Author,
                            Name = bookDto.Name,
                        };

                        _context.Books.Add(book);
                        await SaveAsync();
                    }

                    var bookCopy = new BookCopy
                    {
                        Isbn = bookDto.Isbn,
                        BookId = book.Id,
                        ItemId = item.Id
                    };

                    _context.BookCopies.Add(bookCopy);
                    await SaveAsync();

                    await transaction.CommitAsync();

                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'BookCopy' data created successfully");

                    return bookCopy;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            "Failed adding 'BooksCopy' data");

                    transaction.Rollback();
                    return null;
                }
            }
        }
        public override async Task<BookCopy> UpdateAsync(int id, BookDTO bookDto)
        {
            // Doesn't work as expected
            throw new NotImplementedException();
            /*using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var publisher = await _context.Publishers.FindAsync(bookDto.publisherId);

                    if (publisher == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'Publisher' data with id {bookDto.publisherId} was't found");
                        return new ActionManagerResponse
                        {
                            Success = false,
                            Message = $"Not Found. No entry with id {bookDto.publisherId}",
                            Errors = new List<string> { "Result == null" }
                        };
                    }

                    var bookCopy = await _context.BookCopies.FindAsync(id);

                    if (bookCopy == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'BookCopy' data with id {bookDto.publisherId} was't found");
                        return new ActionManagerResponse
                        {
                            Success = false,
                            Message = $"Not Found. No entry with id {bookDto.publisherId}",
                            Errors = new List<string> { "Result == null" }
                        };
                    }

                    var oldItem = await _context.Items.FindAsync(bookCopy.ItemId);

                    if(oldItem == null)
                    {
                        throw new Exception($"No item exists with id {bookCopy.ItemId}");
                    }

                    _context.Items.Remove(oldItem);
                    await SaveAsync();

                    var item = new Item
                    {
                        PublisherId = publisher.Id,
                    };

                    _context.Items.Add(item);
                    await SaveAsync();

                    var book = await _context.Books.FindAsync(bookCopy.BookId);

                    if (book == null)
                    {
                        throw new Exception($"No book exists with id {bookCopy.BookId}");
                    }

                    book.Name = bookDto.Name;
                    book.Author = bookDto.Author;
                    bookCopy.Isbn = bookDto.Isbn;
                    bookCopy.ItemId = item.Id;

                    await SaveAsync();

                    await transaction.CommitAsync();
                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'BookCopy' data updated successfully");

                    return new ActionManagerResponse<BookCopy>
                    {
                        Success = true,
                        Message = "Data was updated successfully",
                        ActionResult = bookCopy
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"Failed updating 'BookCopy' data [Name = {bookDto.Name}, Author = {bookDto.Author}]");
                    transaction.Rollback();
                    return new ActionManagerResponse
                    {
                        Success = false,
                        Message = $"Instance wasn't updated",
                        Errors = new List<string> { ex.Message }
                    };
                }
            }*/
        }
        public override async Task<BookCopy> DeleteAsync(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var bookCopy = await _context.BookCopies
                        .Include(b => b.Book)
                        .Include(b => b.Item)
                        .FirstOrDefaultAsync(b => b.CopyId == id);

                    if (bookCopy == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'BookCopy' data with id {id} was't found");
                        return null;
                    }

                    _context.Items.Remove(bookCopy.Item);
                    await SaveAsync();

                    if (!bookCopy.Book.BookCopies.Any())
                    {
                        _context.Books.Remove(bookCopy.Book);
                        await SaveAsync();
                    }

                    await transaction.CommitAsync();
                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'BookCopy' data with id {bookCopy.CopyId} was removed successfully");

                    return bookCopy;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            $"Failed deleting 'BookCopy' data [Id = {id}]");

                    transaction.Rollback();
                    return null;
                }
            }
        }

        public override bool CheckModelField(string field)
        {
            if(string.IsNullOrWhiteSpace(field))
            {
                _logger.LogWarning($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            $"Model field is not valid [Field = {field}]");
                return false;
            }

            var listOfNames = new List<string>();

            listOfNames.AddRange(GetPropertyNames<BookCopy>());
            listOfNames.AddRange(GetPropertyNames<Book>());
            listOfNames.AddRange(GetPropertyNames<Item>());
            listOfNames.AddRange(GetPropertyNames<Publisher>());

            if (listOfNames.Contains(field))
            {
                return true;
            }
            else return false;
        }
    }
}
