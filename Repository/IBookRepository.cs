using Library.Models;


namespace Library.Repository
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll();
        Book? GetById(Guid id);
        IEnumerable<Book> SearchByTitle(string title);
        void Add(Book book);
        void Update(Book book);
        void Delete(Guid id);

    }
  }
