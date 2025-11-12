using System;
using System.Collections.Generic;
using Library.Models;


namespace Library.Services
{
    internal interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
        Book? GetBookById(Guid id);
        IEnumerable<Book> SearchBooks(string title);
        Book AddBook(
            string title,
            string isbn,
            int year,
            string genre,
            string language,
            string summary,
            string authorName,
            string nationality
        );
        void UpdateBook(Book book);
        void DeleteBook(Guid id);


    }
}
