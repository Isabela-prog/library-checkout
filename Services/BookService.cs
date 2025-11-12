using Library.Repository;
using Library.Models;
using Library.Utils;

namespace Library.Services
{
    public class BookService : IBookService
    {
            private readonly IBookRepository _bookRepo;

            public BookService(IBookRepository bookRepo)
            {
                _bookRepo = bookRepo;
            }

            public IEnumerable<Book> GetAllBooks() => _bookRepo.GetAll();

            public Book? GetBookById(Guid id) => _bookRepo.GetById(id);

            public IEnumerable<Book> SearchBooks(string title) =>
                _bookRepo.SearchByTitle(title);

            public Book AddBook(
                string title,
                string isbn,
                int year,
                string genre,
                string language,
                string summary,
                string authorName,
                string nationality
            )
            {
                var book = new Book
                {
                    Title = title,
                    Isbn = isbn,
                    Year = year,
                    Genre = genre,
                    Language = language,
                    Summary = summary,
                    AuthorName = authorName,
                    Nationality = nationality
                };

                Validator.ValidateBook(book);

                _bookRepo.Add(book);
                return book;
            }

            public void UpdateBook(Book book) => _bookRepo.Update(book);

            public void DeleteBook(Guid id) => _bookRepo.Delete(id);
        }

    }
