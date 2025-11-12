using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    // Representa um livro no sistema
    public class Book : Author
    {
        public Guid Id { get; set; } = Guid.NewGuid();      // Identificador único do livro
        public string Title { get; set; } = null!;           // Título
        public string Isbn { get; set; } = "";               // ISBN
        public int Year { get; set; }                        // Ano de publicação
        public string Genre { get; set; } = "";              // Gênero
        public string Language { get; set; } = "";           // Idioma
        public string Summary { get; set; } = "";            // Resumo ou sinopse
    }
}
