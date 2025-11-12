using Library.Models;
using System.Text.RegularExpressions;


namespace Library.Utils
{
    public static class Validator
    {
        // Validação completa do usuário
        public static void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username é obrigatório.");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new ArgumentException("Senha é obrigatória.");

            if (string.IsNullOrWhiteSpace(user.FullName))
                throw new ArgumentException("Nome completo é obrigatório.");

            if (string.IsNullOrWhiteSpace(user.Email) || !IsValidEmail(user.Email))
                throw new ArgumentException("Email inválido.");

            if (string.IsNullOrWhiteSpace(user.City))
                throw new ArgumentException("Cidade é obrigatória.");

            if (string.IsNullOrWhiteSpace(user.State))
                throw new ArgumentException("Estado é obrigatório.");

            if (string.IsNullOrWhiteSpace(user.Address))
                throw new ArgumentException("Endereço é obrigatório.");
        }

        // Validação completa do livro
        public static void ValidateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Título do livro é obrigatório.");

            if (string.IsNullOrWhiteSpace(book.Isbn))
                throw new ArgumentException("ISBN é obrigatório.");

            if (string.IsNullOrWhiteSpace(book.Genre))
                throw new ArgumentException("Gênero é obrigatório.");

            if (string.IsNullOrWhiteSpace(book.Language))
                throw new ArgumentException("Idioma é obrigatório.");

            if (string.IsNullOrWhiteSpace(book.Summary))
                throw new ArgumentException("Resumo é obrigatório.");

            if (string.IsNullOrWhiteSpace(book.AuthorName))
                throw new ArgumentException("Nome do autor é obrigatório.");

            if (!string.IsNullOrWhiteSpace(book.Isbn) && !IsValidIsbn(book.Isbn))
                throw new ArgumentException("ISBN inválido.");
        }

        // Validação de email com regex simples
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // Validação de ISBN (10 ou 13 dígitos)
        public static bool IsValidIsbn(string isbn)
        {
            return Regex.IsMatch(isbn, @"^\d{10}(\d{3})?$");
        }
    }
}