using Library.Repository;
using Library.Services;
using Library.Security;
using Library.Utils;
using Library.Models;

class Program
{
    static void Main()
    {
        var repoPath = "C:\\Users\\isant\\OneDrive\\Área de Trabalho\\treinamento\\Library\\Data\\users.json";
        var userRepo = new JsonUserRepository(repoPath);
        var hasher = new Pbkdf2PasswordHasher();
        var geoService = new GeoService();
        var authService = new AuthService(userRepo, hasher, geoService);

        while (true)
        {
            Console.WriteLine("\n=== MENU ===");
            Console.WriteLine("1. Registrar");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Excluir conta");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Registrar(authService);
                    break;
                case "2":
                    Logar(authService);
                    break;
                case "3":
                    Excluir(userRepo, hasher);
                    break;
                case "0":
                    Console.WriteLine("Encerrando...");
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    static void Registrar(AuthService auth)
    {
        Console.WriteLine("\n=== Registro de Usuário ===");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();
        Console.Write("Nome completo: ");
        var fullName = Console.ReadLine();
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Cidade: ");
        var city = Console.ReadLine();
        Console.Write("Estado: ");
        var state = Console.ReadLine();
        Console.Write("Endereço completo: ");
        var address = Console.ReadLine();

        try
        {
            var user = auth.Register(username!, password!, fullName!, email!, city!, state!, address!);
            Console.WriteLine($"Usuário {user.Username} registrado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao registrar: {ex.Message}");
        }
    }

    static void Logar(AuthService auth)
    {
        Console.WriteLine("\n=== Login ===");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        var user = auth.Authenticate(username!, password!);
        if (user != null)
        {
            Console.WriteLine($"Login bem-sucedido! Bem-vindo(a), {user.FullName}");
        }
        else
        {
            Console.WriteLine("Usuário ou senha inválidos.");
        }
    }

    static void Excluir(IUserRepository repo, IPasswordHasher hasher)
    {
        Console.WriteLine("\n=== Excluir Conta ===");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        var user = repo.GetByUsername(username!);
        if (user == null || !hasher.Verify(password!, user.PasswordHash))
        {
            Console.WriteLine("Usuário ou senha inválidos.");
            return;
        }

        repo.Delete(user.Id);
        Console.WriteLine($"Conta de {user.Username} excluída com sucesso.");
    }
}