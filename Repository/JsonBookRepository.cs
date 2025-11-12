using Library.Models;
using System.Text.Json;

namespace Library.Repository
{
    internal class JsonBookRepository : IBookRepository
    {
        private readonly string _path;

        public JsonBookRepository(string path)
        {
            _path = path;
            Directory.CreateDirectory(Path.GetDirectoryName(_path) ?? ".");
            if (!File.Exists(_path)) File.WriteAllText(_path, "[]");
        }

        private List<Book> LoadAll()
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
        }

        private void SaveAll(List<Book> books)
        {
            var json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json);
        }

        public IEnumerable<Book> GetAll() => LoadAll();

        public Book? GetById(Guid id) => LoadAll().FirstOrDefault(b => b.Id == id);

        public IEnumerable<Book> SearchByTitle(string title) =>
            LoadAll().Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        public void Add(Book book)
        {
            var list = LoadAll();
            list.Add(book);
            SaveAll(list);
        }

        public void Update(Book book)
        {
            var list = LoadAll();
            var idx = list.FindIndex(b => b.Id == book.Id);
            if (idx >= 0) list[idx] = book;
            else list.Add(book);
            SaveAll(list);
        }

        public void Delete(Guid id)
        {
            var list = LoadAll();
            list.RemoveAll(b => b.Id == id);
            SaveAll(list);
        }
    }
}
