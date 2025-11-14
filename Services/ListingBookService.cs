using Library.Models;
using Library.Repository;
using Library.Utils;

namespace Library.Services
{
    public class ListingBookService
    {
        private readonly ListingBookRepository _repo;

        public ListingBookService(ListingBookRepository repo)
        {
            _repo = repo;
        }

        public void AddBook(ListingBook book)
        {
            Validator.ValidateListingBook(book);
            _repo.Add(book);
        }

        public bool RemoveBook(Guid userId, string title, string author)
        {
            var match = _repo.FindByTitleAndAuthor(userId, title, author);
            if (match == null) return false;

            _repo.Delete(match.Id);
            return true;
        }

        public List<ListingBook> GetBooksByUser(Guid userId)
        {
            return _repo.GetByUser(userId);
        }

        // Filtro por gênero
        public List<ListingBook> FilterByGenre(Guid userId, string genre)
        {
            return _repo.GetByUser(userId)
                        .Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                        .ToList();
        }

        // Filtro por idioma
        public List<ListingBook> FilterByLanguage(Guid userId, string language)
        {
            return _repo.GetByUser(userId)
                        .Where(b => b.Language.Equals(language, StringComparison.OrdinalIgnoreCase))
                        .ToList();
        }

        // Filtro por autor
        public List<ListingBook> FilterByAuthor(Guid userId, string author)
        {
            return _repo.GetByUser(userId)
                        .Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                        .ToList();
        }
    }
}