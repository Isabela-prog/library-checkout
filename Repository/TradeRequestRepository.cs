using Library.Models;
using System.Text.Json;


namespace Library.Repository
{
    public class TradeRequestRepository
    {
        private readonly string _path;
        public string Path => _path;

        private List<TradeRequest> _requests = new();

        public TradeRequestRepository(string path)
        {
            _path = path;

            var directory = System.IO.Path.GetDirectoryName(_path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory!);

            if (File.Exists(_path))
            {
                var json = File.ReadAllText(_path);
                if (!string.IsNullOrWhiteSpace(json))
                    _requests = JsonSerializer.Deserialize<List<TradeRequest>>(json) ?? new();
            }
        }


        public void Add(TradeRequest request)
        {
            _requests.Add(request);
            SaveAll();
        }

        public List<TradeRequest> GetByOwner(Guid ownerId) =>
            _requests.Where(r => r.OwnerId == ownerId).ToList();

        public List<TradeRequest> GetByRequester(Guid requesterId) =>
            _requests.Where(r => r.RequesterId == requesterId).ToList();

        public TradeRequest? GetById(Guid id) =>
            _requests.FirstOrDefault(r => r.Id == id);

        public void UpdateStatus(Guid requestId, string status)
        {
            var request = GetById(requestId);
            if (request != null)
            {
                request.Status = status;
                SaveAll();
            }
        }

        private void SaveAll()
        {
            var json = JsonSerializer.Serialize(_requests, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json);
        }


    }
}