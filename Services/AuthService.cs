using Library.Repository;
using Library.Models;
using Library.Security;
using Library.Utils;

namespace Library.Services
{
    // Serviço que gerencia registro e login
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _hasher;
        private readonly IGeoService _geoService;

        public AuthService(IUserRepository userRepo, IPasswordHasher hasher, IGeoService geoService)
        {
            _userRepo = userRepo;
            _hasher = hasher;
            _geoService = geoService;
        }

        public User Register(string username, string password, string fullName, string email, string city, string state, string address)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username obrigatório.");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Senha obrigatória.");
            if (_userRepo.GetByUsername(username) != null) throw new InvalidOperationException("Usuário já existe.");

            var user = new User
            {
                Id = Guid.NewGuid(), 
                Username = username,
                PasswordHash = _hasher.Hash(password),
                FullName = fullName,
                Email = email,
                City = city,
                State = state,
                Address = address
            };

            // Geocodificação automática
            var fullAddress = $"{address}, {city}, {state}, Brasil"; // ← Inclui país para melhorar precisão
            var coords = _geoService.GetCoordinates(fullAddress);
            if (coords != null)
            {
                user.Latitude = coords.Value.Latitude;
                user.Longitude = coords.Value.Longitude;
            }

            // Validação completa
            Validator.ValidateUser(user);

            _userRepo.Add(user);
            return user;
        }

        public User? Authenticate(string username, string password)
        {
            var user = _userRepo.GetByUsername(username);
            if (user == null) return null;
            if (!_hasher.Verify(password, user.PasswordHash)) return null;
            return user;
        }

        public void UpdateUser(User user)
        {
            _userRepo.Update(user);
        }
    }
}