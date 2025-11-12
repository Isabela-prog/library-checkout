using Library.Models;
using System.Text.Json;

namespace Library.Repository
{
    // Implementação que salva usuários em arquivo JSON
    public class JsonUserRepository : IUserRepository
    {
        private readonly string _path;

        public JsonUserRepository(string path)
        {
            _path = path;

            // Garante que o diretório existe
            var directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            // Inicializa o arquivo com uma lista vazia se não existir
            if (!File.Exists(_path))
                File.WriteAllText(_path, "[]");
        }

        public List<User> LoadAll()
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public void SaveAll(List<User> users)
        {
            Console.WriteLine($"Salvando {users.Count} usuários em {_path}");
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json);
        }

        public IEnumerable<User> GetAll() => LoadAll();

        public User? GetById(Guid id) => LoadAll().FirstOrDefault(u => u.Id == id);

        public User? GetByUsername(string username) =>
            LoadAll().FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

        public void Add(User user)
        {
            var list = LoadAll();
            list.Add(user);
            SaveAll(list);
        }

        public void Update(User user)
        {
            var list = LoadAll();
            var idx = list.FindIndex(u => u.Id == user.Id);
            if (idx >= 0) list[idx] = user;
            else list.Add(user);
            SaveAll(list);
        }

        public void Delete(Guid id)
        {
            var list = LoadAll();
            list.RemoveAll(u => u.Id == id);
            SaveAll(list);
        }
    }
}