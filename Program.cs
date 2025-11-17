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
                    Logar(authService, bookService, bookRepo, tradeService, userRepo, tradeRepo);
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

    static void Logar(AuthService auth, ListingBookService bookService, ListingBookRepository bookRepo, TradeService tradeService, JsonUserRepository userRepo, TradeRequestRepository tradeRepo)
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
                    AdicionarLivro(user, bookService);
                    break;

                case "2":
                    MostrarMeusLivros(user, bookService, userRepo);
                    break;

                case "3":
                    RemoverLivro(user, bookService);
                    break;

                case "4":
                    SolicitarTrocaFlow(user, bookService, bookRepo, tradeService, userRepo);
                    break;

                case "5":
                    MostrarSolicitacoesEnviadas(user, tradeService, bookRepo, userRepo);
                    break;

                case "6":
                    MostrarSolicitacoesRecebidas(user, tradeService, bookRepo, userRepo);
                    break;

                case "7":
                    GerenciarSolicitacoesRecebidas(user, tradeService, bookRepo, userRepo);
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    static void AdicionarLivro(User user, ListingBookService bookService)
    {
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
            return;
        }

        var book = new ListingBook
        {
            Id = Guid.NewGuid(),
            OwnerId = user.Id,
            Title = title ?? "",
            Author = author ?? "",
            Condition = condition ?? "",
            Year = year,
            Genre = genre ?? "",
            Language = language ?? "",
            Summary = summary ?? ""
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
    }

    static void MostrarMeusLivros(User user, ListingBookService bookService, JsonUserRepository userRepo)
    {
        var myBooks = bookService.GetBooksByUser(user.Id);
        Console.WriteLine("\nMeus livros disponíveis para troca:");
        if (myBooks.Count == 0)
        {
            Console.WriteLine("Nenhum livro listado.");
            return;
        }

        // Formato A: 1 — Título — Autor — Dono: Nome — Cond.: Condição
        for (int i = 0; i < myBooks.Count; i++)
        {
            var b = myBooks[i];
            var owner = userRepo.GetById(b.OwnerId);
            var ownerName = owner != null ? owner.FullName : "Desconhecido";
            Console.WriteLine($"{i + 1} — {b.Title} — {b.Author} — Dono: {ownerName} — Cond.: {b.Condition}");
        }
    }

    static void RemoverLivro(User user, ListingBookService bookService)
    {
        Console.Write("Título do livro: ");
        var titleToRemove = Console.ReadLine();
        Console.Write("Autor do livro: ");
        var authorToRemove = Console.ReadLine();

        var removed = bookService.RemoveBook(user.Id, titleToRemove ?? "", authorToRemove ?? "");
        if (removed)
            Console.WriteLine("Livro removido da troca.");
        else
            Console.WriteLine("Nenhum livro encontrado com esse título e autor.");
    }

    static void SolicitarTrocaFlow(User user, ListingBookService bookService, ListingBookRepository bookRepo, TradeService tradeService, JsonUserRepository userRepo)
    {
        Console.WriteLine("\n=== Solicitar Troca ===");
        Console.Write("Título do livro que deseja solicitar: ");
        var requestedTitle = Console.ReadLine();
        Console.Write("Autor do livro: ");
        var requestedAuthor = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(requestedTitle) || string.IsNullOrWhiteSpace(requestedAuthor))
        {
            Console.WriteLine("Título e autor são obrigatórios.");
            return;
        }

        var matches = bookService.FindAllByTitleAndAuthor(requestedTitle!, requestedAuthor!);
        // filtra para não incluir os livros do próprio usuário
        matches = matches.Where(b => b.OwnerId != user.Id).ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine("Nenhum livro encontrado de outros usuários com esse título e autor.");
            return;
        }

        Console.WriteLine($"\nForam encontrados {matches.Count} livro(s):\n");
        for (int i = 0; i < matches.Count; i++)
        {
            var b = matches[i];
            var owner = userRepo.GetById(b.OwnerId);
            var ownerName = owner != null ? owner.FullName : "Desconhecido";
            Console.WriteLine($"{i + 1} — {b.Title} — {b.Author} — Dono: {ownerName} — Cond.: {b.Condition}");
        }

        Console.Write("\nEscolha o número do livro que deseja solicitar: ");
        var choiceStr = Console.ReadLine();
        if (!int.TryParse(choiceStr, out var choice) || choice < 1 || choice > matches.Count)
        {
            Console.WriteLine("Escolha inválida.");
            return;
        }

        var selected = matches[choice - 1];

        // Pergunta se quer oferecer um livro
        Console.Write("Deseja oferecer um livro em troca? (s/n): ");
        var offerChoice = Console.ReadLine();
        Guid? offeredId = null;

        if (offerChoice?.ToLower() == "s")
        {
            var userBooks = bookService.GetBooksByUser(user.Id);
            if (userBooks.Count == 0)
            {
                Console.WriteLine("Você não possui livros para oferecer.");
            }
            else
            {
                Console.WriteLine("\nSeus livros disponíveis:");
                for (int i = 0; i < userBooks.Count; i++)
                {
                    var b = userBooks[i];
                    Console.WriteLine($"{i + 1} — {b.Title} — {b.Author} — Cond.: {b.Condition}");
                }

                Console.Write("Escolha o número do livro que deseja oferecer: ");
                var offeredChoice = Console.ReadLine();
                if (int.TryParse(offeredChoice, out var offIdx) && offIdx >= 1 && offIdx <= userBooks.Count)
                {
                    offeredId = userBooks[offIdx - 1].Id;
                }
                else
                {
                    Console.WriteLine("Opção de livro oferecido inválida. Nenhum livro será oferecido.");
                }
            }
        }

        tradeService.SolicitarTroca(user.Id, selected.OwnerId, selected.Id, offeredId);
        Console.WriteLine("Solicitação enviada.");
    }

    static void MostrarSolicitacoesEnviadas(User user, TradeService tradeService, ListingBookRepository bookRepo, JsonUserRepository userRepo)
    {
        var enviadas = tradeService.ListarSolicitacoesEnviadas(user.Id);
        Console.WriteLine("\nSolicitações enviadas:");
        if (enviadas.Count == 0)
        {
            Console.WriteLine("Nenhuma solicitação enviada.");
            return;
        }

        // Mostra com formato A para o livro (se existir), e sempre mostra o dono (owner) mesmo que o livro tenha sido removido
        for (int i = 0; i < enviadas.Count; i++)
        {
            var r = enviadas[i];
            var livro = bookRepo.GetAll().FirstOrDefault(b => b.Id == r.BookRequestedId);
            var owner = userRepo.GetById(r.OwnerId);
            var ownerName = owner != null ? owner.FullName : "Desconhecido";

            if (livro != null)
            {
                Console.WriteLine($"{i + 1} — {livro.Title} — {livro.Author} — Dono: {ownerName} — Status: {r.Status}");
            }
            else
            {
                // Livro possivelmente removido (já trocado) — mostra ID do livro e dono
                Console.WriteLine($"{i + 1} — Livro removido (ID: {r.BookRequestedId}) — Dono: {ownerName} — Status: {r.Status}");
            }
        }
    }

    static void MostrarSolicitacoesRecebidas(User user, TradeService tradeService, ListingBookRepository bookRepo, JsonUserRepository userRepo)
    {
        var recebidas = tradeService.ListarSolicitacoesRecebidas(user.Id);
        Console.WriteLine("\nSolicitações recebidas (pendentes):");
        if (recebidas.Count == 0)
        {
            Console.WriteLine("Nenhuma solicitação pendente.");
            return;
        }

        for (int i = 0; i < recebidas.Count; i++)
        {
            var r = recebidas[i];
            var livro = bookRepo.GetAll().FirstOrDefault(b => b.Id == r.BookRequestedId);
            var requester = userRepo.GetById(r.RequesterId);
            var requesterName = requester != null ? requester.FullName : "Desconhecido";

            if (livro != null)
            {
                Console.WriteLine($"{i + 1} — {livro.Title} — {livro.Author} — Solicitante: {requesterName} — Status: {r.Status}");
            }
            else
            {
                Console.WriteLine($"{i + 1} — Livro removido (ID: {r.BookRequestedId}) — Solicitante: {requesterName} — Status: {r.Status}");
            }
        }
    }

    static void GerenciarSolicitacoesRecebidas(User user, TradeService tradeService, ListingBookRepository bookRepo, JsonUserRepository userRepo)
    {
        var recebidas = tradeService.ListarSolicitacoesRecebidas(user.Id);
        if (recebidas.Count == 0)
        {
            Console.WriteLine("\nNenhuma solicitação pendente para gerenciar.");
            return;
        }

        Console.WriteLine("\n=== Gerenciar Solicitações Recebidas ===");
        // Lista numerada igual ao método MostrarSolicitacoesRecebidas
        for (int i = 0; i < recebidas.Count; i++)
        {
            var r = recebidas[i];
            var livro = bookRepo.GetAll().FirstOrDefault(b => b.Id == r.BookRequestedId);
            var requester = userRepo.GetById(r.RequesterId);
            var requesterName = requester != null ? requester.FullName : "Desconhecido";

            if (livro != null)
                Console.WriteLine($"{i + 1} — {livro.Title} — {livro.Author} — Solicitante: {requesterName} — Status: {r.Status}");
            else
                Console.WriteLine($"{i + 1} — Livro removido (ID: {r.BookRequestedId}) — Solicitante: {requesterName} — Status: {r.Status}");
        }

        Console.Write("\nEscolha o número da solicitação que deseja gerenciar: ");
        var sel = Console.ReadLine();
        if (!int.TryParse(sel, out var selIdx) || selIdx < 1 || selIdx > recebidas.Count)
        {
            Console.WriteLine("Escolha inválida.");
            return;
        }

        var solicitacao = recebidas[selIdx - 1];

        Console.Write("Aceitar ou recusar? (a/r): ");
        var decision = Console.ReadLine();

        if (decision?.ToLower() == "a")
        {
            var ok = tradeService.AceitarTroca(solicitacao.Id);
            Console.WriteLine(ok ? "Troca aceita." : "Erro ao aceitar (talvez a solicitação não esteja mais pendente).");
        }
        else if (decision?.ToLower() == "r")
        {
            var ok = tradeService.RecusarTroca(solicitacao.Id);
            Console.WriteLine(ok ? "Troca recusada." : "Erro ao recusar (talvez a solicitação não esteja mais pendente).");
        }
        else
        {
            Console.WriteLine("Opção inválida.");
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
