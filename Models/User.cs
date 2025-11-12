namespace Library.Models
{
    // Representa um usuário
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Address { get; set; } = "";
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Dictionary<string, string> Preferences { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
