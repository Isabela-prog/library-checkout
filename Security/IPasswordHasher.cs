namespace Library.Security
{
    // Interface para abstrair o algoritmo de hash de senha
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
