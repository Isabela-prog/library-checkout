using Library.Models;
using Library.Data;
using System.Collections.Generic;
using System.Linq;


namespace Library.Data
{
    public class BookRepository(JsonContext context)
    {
        private readonly string _filePath = "books.json";
        private readonly JsonContext _context = context;


        public List<Book> GetAll()
        {
            return _context.Load<Book>(_filePath);
        }

        public Book? GetById(int id) 
        {             
            var books = GetAll();
            return books.FirstOrDefault(b => b.Id == id);
        }

        public void Add(Book book)
        {
            var books = GetAll();
            books.Add(book);
            _context.Save(_filePath, books);
        }

        public void Update(Book book)
        {
            var books = GetAll();
            var index = books.FindIndex(b => b.Id == book.Id);
            if (index >= 0)
            {
                books[index] = book;
                _context.Save(_filePath, books);
            }
        }

        public void Remove(int id)
        {
            var books = GetAll();
            var bookToRemove = books.FirstOrDefault(b => b.Id == id);
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                _context.Save(_filePath, books);
            }
        }
    }
}
