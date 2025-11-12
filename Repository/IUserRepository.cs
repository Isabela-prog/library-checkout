using Library.Models;

namespace Library.Repository
{
    // Interface para persistência de usuários
    public interface IUserRepository
    {
        IEnumerable<User> GetAll(); // Retorna todos os usuários
        User? GetById(Guid id);     // Busca por ID
        User? GetByUsername(string username); // Busca por nome de login
        void Add(User user);        // Adiciona novo usuário
        void Update(User user);     // Atualiza dados de usuário
        void Delete(Guid id);       // Remove usuário

    }
}
