namespace Library.Models
{
    public class TradeRequest
    {
        public Guid Id { get; set; }
        public Guid RequesterId { get; set; }       // Quem está solicitando a troca
        public Guid OwnerId { get; set; }           // Dono do livro solicitado
        public Guid BookRequestedId { get; set; }   // Livro que o solicitante quer
        public Guid? BookOfferedId { get; set; }    // Livro que está oferecendo (opcional)
        public string Status { get; set; } = "Pendente"; // Pendente, Aceita, Recusada
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}