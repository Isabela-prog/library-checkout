using Library.Models;
using System.Text.Json;

namespace Library.Repository
{
    public class ListingBookRepository
    {
        private readonly string _path;
        private List<ListingBook> _books = new();

        public ListingBookRepository(string path)
        {
            _path = path;

            if (!File.Exists(_path))
                Directory.CreateDirectory(Path.GetDirectoryName(_path)!);

            if (File.Exists(_path))
            {
                var json = File.ReadAllText(_path);
                if (!string.IsNullOrWhiteSpace(json))
                    _books = JsonSerializer.Deserialize<List<ListingBook>>(json) ?? new();
            }
        }

        public void Add(ListingBook book)
        {
            _books.Add(book);
            SaveAll();
        }

        public void Delete(Guid id)
        {
            _books.RemoveAll(b => b.Id == id);
            SaveAll();
        }

        public List<ListingBook> GetByUser(Guid userId) =>
            _books.Where(b => b.OwnerId == userId).ToList();

        public ListingBook? FindByTitleAndAuthor(Guid userId, string title, string author) =>
            _books.FirstOrDefault(b =>
                b.OwnerId == userId &&
                b.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                b.Author.Equals(author, StringComparison.OrdinalIgnoreCase));

        private void SaveAll()
        {
            var json = JsonSerializer.Serialize(_books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json);
        }

        public List<ListingBook> GetAll() => _books;
    }
}