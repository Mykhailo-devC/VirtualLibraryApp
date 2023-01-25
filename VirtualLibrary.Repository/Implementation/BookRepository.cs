﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using VirtualLibrary.Models;

namespace VirtualLibrary.Repository.Implementation
{
    public class BookRepository : RepositoryBase<Book, BookDTO>
    {
        public BookRepository(VirtualLibraryDbContext context, ILogger<RepositoryBase<Book, BookDTO>> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Book>> GetAllAsync()
        {
            var books = await _context.Books
                    .Include(e => e.BookCopies)
                    .ThenInclude(e => e.Item)
                    .ThenInclude(e => e.Publisher)
                    .ToListAsync();

            _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        "'Book' data was read successfully");

            return books;
        }
        public override async Task<Book> GetByIdAsync(int id)
        {
            //Tip: it would use to return all copies of a book
            try
            {
                var book = await _context.Books
                    .Include(b => b.BookCopies)
                    .ThenInclude(b => b.Item)
                    .ThenInclude(b => b.Publisher)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                {
                    _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Book' data with id {id} was't found");
                    return null;
                }

                _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"'Book' data read successfully");

                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                        $"Failed read 'Book' data [Id = {id}]");
                return null;
            }
        }

        public override async Task<Book> CreateAsync(BookDTO bookDto)
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
                        "'Book' data created successfully");

                    return book;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            "Failed adding 'Books' data");

                    transaction.Rollback();
                    return null;
                }
            }
        }
        public override async Task<Book> UpdateAsync(int id, BookDTO bookDto)
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
        public override async Task<Book> DeleteAsync(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var book = await _context.Books
                        .Include(b => b.BookCopies)
                        .ThenInclude(b => b.Item)
                        .FirstOrDefaultAsync(b => b.Id == id);

                    if (book == null)
                    {
                        _logger.LogWarning($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'Book' data with id {id} was't found");
                        return null;
                    }

                    var bookCopy = book.BookCopies.FirstOrDefault();

                    if (bookCopy == null)
                    {
                        throw new Exception($"Book with id {book.Id} has no BookCopies");
                    }
                    _context.Items.Remove(bookCopy.Item);
                    _context.BookCopies.Remove(bookCopy);
                    await SaveAsync();

                    if (!book.BookCopies.Any())
                    {
                        _context.Books.Remove(book);
                        await SaveAsync();
                    }

                    await transaction.CommitAsync();
                    _logger.LogInformation($"|{GetType().Name}.{MethodBase.GetCurrentMethod().Name}|" +
                            $"'BookCopy' data with id {bookCopy.CopyId} was removed successfully");

                    return book;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}" +
                            $"Failed deleting 'Book' data [Id = {id}]");

                    transaction.Rollback();
                    return null;
                }
            }
        }
    }
}