namespace Library.Models
{
    public class ListingBook
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; } // Identificador do usuário que listou o livro
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public string Condition { get; set; } = ""; // Novo, Usado, etc.
        public int Year { get; set; } // Ano de publicação
        public string Genre { get; set; } = ""; // Gênero
        public string Language { get; set; } = ""; // Idioma
        public string Summary { get; set; } = ""; // Resumo ou sinopse
    }
}