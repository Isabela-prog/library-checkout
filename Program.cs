using Library.Repository;
using Library.Services;
using Library.Security;
using Library.Utils;
using Library.Models;

class Program
{
    static void Main()
    {
        var userPath = "C:\\Users\\isant\\OneDrive\\Área de Trabalho\\treinamento\\Library\\Data\\users.json";
        var booksPath = "C:\\Users\\isant\\OneDrive\\Área de Trabalho\\treinamento\\Library\\Data\\listingsBook.json";
        var tradePath = "C:\\Users\\isant\\OneDrive\\Área de Trabalho\\treinamento\\Library\\Data\\tradeRequests.json";
       
        

        var userRepo = new JsonUserRepository(userPath);
        var bookRepo = new ListingBookRepository(booksPath);
        var tradeRepo = new TradeRequestRepository(tradePath);

        var bookService = new ListingBookService(bookRepo);
        var tradeService = new TradeService(tradeRepo, bookRepo);

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
                    Logar(authService, bookService, bookRepo, tradeService);
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
            Console.WriteLine($"Usuário {user.Username} registrado com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao registrar: {ex.Message}");
        }
    }

    static void Logar(AuthService auth, ListingBookService bookService, ListingBookRepository bookRepo, TradeService tradeService)
    {
        Console.WriteLine("\n=== Login ===");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        var user = auth.Authenticate(username!, password!);
        if (user == null)
        {
            Console.WriteLine("Usuário ou senha inválidos.");
            return;
        }

        Console.WriteLine($"Login bem-sucedido. Bem-vindo(a), {user.FullName}");

        while (true)
        {
            Console.WriteLine("\n--- MENU DO USUÁRIO ---");
            Console.WriteLine("1. Adicionar livro para troca");
            Console.WriteLine("2. Ver meus livros listados");
            Console.WriteLine("3. Remover um livro da troca");
            Console.WriteLine("4. Solicitar troca");
            Console.WriteLine("5. Ver solicitações enviadas");
            Console.WriteLine("6. Ver solicitações recebidas");
            Console.WriteLine("7. Gerenciar solicitações recebidas");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");
            var opt = Console.ReadLine();

            switch (opt)
            {
                case "1":
                    Console.WriteLine("\n=== Adicionar Livro para Troca ===");
                    Console.Write("Título: ");
                    var title = Console.ReadLine();
                    Console.Write("Autor: ");
                    var author = Console.ReadLine();
                    Console.Write("Condição (Novo, Usado, etc.): ");
                    var condition = Console.ReadLine();
                    Console.Write("Ano de publicação: ");
                    var yearStr = Console.ReadLine();
                    Console.Write("Gênero: ");
                    var genre = Console.ReadLine();
                    Console.Write("Idioma: ");
                    var language = Console.ReadLine();
                    Console.Write("Sinopse: ");
                    var summary = Console.ReadLine();

                    if (!int.TryParse(yearStr, out var year))
                    {
                        Console.WriteLine("Ano inválido.");
                        break;
                    }

                    var book = new ListingBook
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = user.Id,
                        Title = title!,
                        Author = author!,
                        Condition = condition!,
                        Year = year,
                        Genre = genre!,
                        Language = language!,
                        Summary = summary!
                    };

                    try
                    {
                        Validator.ValidateListingBook(book);
                        bookService.AddBook(book);
                        Console.WriteLine("Livro adicionado para troca.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao adicionar livro: {ex.Message}");
                    }
                    break;

                case "2":
                    var myBooks = bookService.GetBooksByUser(user.Id);
                    Console.WriteLine("\nMeus livros disponíveis para troca:");
                    if (myBooks.Count == 0)
                        Console.WriteLine("Nenhum livro listado.");
                    else
                        myBooks.ForEach(b => Console.WriteLine($"{b.Title} — {b.Author}"));
                    break;

                case "3":
                    Console.Write("Título do livro: ");
                    var titleToRemove = Console.ReadLine();
                    Console.Write("Autor do livro: ");
                    var authorToRemove = Console.ReadLine();

                    var removed = bookService.RemoveBook(user.Id, titleToRemove!, authorToRemove!);
                    if (removed)
                        Console.WriteLine("Livro removido da troca.");
                    else
                        Console.WriteLine("Nenhum livro encontrado com esse título e autor.");
                    break;
                case "4":
                    Console.WriteLine("\n=== Solicitar Troca ===");
                    Console.Write("Título do livro que deseja solicitar: ");
                    var requestedTitle = Console.ReadLine();
                    Console.Write("Autor do livro: ");
                    var requestedAuthor = Console.ReadLine();

                    var allBooks = bookRepo.GetAll();
                    var bookRequested = allBooks.FirstOrDefault(b =>
                        b.Title.Equals(requestedTitle, StringComparison.OrdinalIgnoreCase) &&
                        b.Author.Equals(requestedAuthor, StringComparison.OrdinalIgnoreCase) &&
                        b.OwnerId != user.Id);

                    if (bookRequested == null)
                    {
                        Console.WriteLine("Livro não encontrado ou pertence a você.");
                        break;
                    }

                    Console.Write("Deseja oferecer um livro em troca? (s/n): ");
                    var offerChoice = Console.ReadLine();
                    Guid? offeredId = null;

                    if (offerChoice?.ToLower() == "s")
                    {
                        var userBooks = bookService.GetBooksByUser(user.Id); // Renomeado de myBooks para userBooks
                        if (userBooks.Count == 0)
                        {
                            Console.WriteLine("Você não possui livros para oferecer.");
                            break;
                        }

                        Console.WriteLine("Seus livros disponíveis:");
                        foreach (var b in userBooks)
                            Console.WriteLine($"- {b.Title} — {b.Author}");

                        Console.Write("Título do livro que deseja oferecer: ");
                        var offeredTitle = Console.ReadLine();
                        Console.Write("Autor do livro oferecido: ");
                        var offeredAuthor = Console.ReadLine();

                        var bookOffered = userBooks.FirstOrDefault(b =>
                            b.Title.Equals(offeredTitle, StringComparison.OrdinalIgnoreCase) &&
                            b.Author.Equals(offeredAuthor, StringComparison.OrdinalIgnoreCase));

                        if (bookOffered == null)
                        {
                            Console.WriteLine("Livro oferecido não encontrado.");
                            break;
                        }

                        offeredId = bookOffered.Id;
                    }

                    tradeService.SolicitarTroca(user.Id, bookRequested.OwnerId, bookRequested.Id, offeredId);
                    Console.WriteLine("Solicitação enviada.");
                    break;

                case "5":
                    var enviadas = tradeService.ListarSolicitacoesEnviadas(user.Id);
                    Console.WriteLine("\nSolicitações enviadas:");
                    if (enviadas.Count == 0)
                    {
                        Console.WriteLine("Nenhuma solicitação enviada.");
                        break;
                    }

                    foreach (var r in enviadas)
                    {
                        var livroSolicitado = bookService.GetBooksByUser(r.OwnerId).FirstOrDefault(b => b.Id == r.BookRequestedId);
                        if (livroSolicitado != null)
                            Console.WriteLine($"{livroSolicitado.Title} — {livroSolicitado.Author} | Status: {r.Status}");
                    }
                    break;

                case "6":
                    var recebidas = tradeService.ListarSolicitacoesRecebidas(user.Id);
                    Console.WriteLine("\nSolicitações recebidas:");
                    if (recebidas.Count == 0)
                    {
                        Console.WriteLine("Nenhuma solicitação pendente.");
                        break;
                    }

                    foreach (var r in recebidas)
                    {
                        var livroSolicitado = bookService.GetBooksByUser(user.Id).FirstOrDefault(b => b.Id == r.BookRequestedId);
                        if (livroSolicitado != null)
                            Console.WriteLine($"{livroSolicitado.Title} — {livroSolicitado.Author}");
                    }
                    break;

                case "7":
                    Console.WriteLine("\n=== Gerenciar Solicitações Recebidas ===");
                    Console.Write("Título do livro solicitado: ");
                    var requestedTitle_gerenciar = Console.ReadLine();
                    Console.Write("Autor do livro solicitado: ");
                    var requestedAuthor_gerenciar = Console.ReadLine();

                    var recebidasGerenciar = tradeService.ListarSolicitacoesRecebidas(user.Id);
                    var livro = bookRepo.GetByUser(user.Id).FirstOrDefault(b =>
                        b.Title.Equals(requestedTitle_gerenciar, StringComparison.OrdinalIgnoreCase) &&
                        b.Author.Equals(requestedAuthor_gerenciar, StringComparison.OrdinalIgnoreCase));

                    if (livro == null)
                    {
                        Console.WriteLine("Livro não encontrado entre seus listados.");
                        break;
                    }

                    var solicitacao = recebidasGerenciar.FirstOrDefault(r => r.BookRequestedId == livro.Id);
                    if (solicitacao == null)
                    {
                        Console.WriteLine("Nenhuma solicitação pendente para esse livro.");
                        break;
                    }

                    Console.Write("Aceitar ou recusar? (a/r): ");
                    var decision = Console.ReadLine();

                    if (decision?.ToLower() == "a")
                    {
                        var ok = tradeService.AceitarTroca(solicitacao.Id);
                        Console.WriteLine(ok ? "Troca aceita." : "Erro ao aceitar.");
                    }
                    else if (decision?.ToLower() == "r")
                    {
                        var ok = tradeService.RecusarTroca(solicitacao.Id);
                        Console.WriteLine(ok ? "Troca recusada." : "Erro ao recusar.");
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida.");
                    }
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
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