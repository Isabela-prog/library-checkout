using Library.Models;
using Library.Repository;

namespace Library.Services
{
    public class TradeService
    {
        private readonly TradeRequestRepository _tradeRepo;
        private readonly ListingBookRepository _bookRepo;

        public TradeService(TradeRequestRepository tradeRepo, ListingBookRepository bookRepo)
        {
            _tradeRepo = tradeRepo;
            _bookRepo = bookRepo;
        }

        // Solicita uma troca
        public void SolicitarTroca(Guid requesterId, Guid ownerId, Guid bookRequestedId, Guid? bookOfferedId = null)
        {
            var request = new TradeRequest
            {
                Id = Guid.NewGuid(),
                RequesterId = requesterId,
                OwnerId = ownerId,
                BookRequestedId = bookRequestedId,
                BookOfferedId = bookOfferedId
            };

            _tradeRepo.Add(request);
        }

        private void RegistrarHistorico(TradeRequest original, Guid destinoId, string novoStatus)
        {
            var historico = new TradeRequest
            {
                Id = Guid.NewGuid(),
                RequesterId = original.RequesterId,
                OwnerId = original.OwnerId,
                BookRequestedId = original.BookRequestedId,
                BookOfferedId = original.BookOfferedId,
                Status = novoStatus,
                CreatedAt = DateTime.Now
            };

            _tradeRepo.Add(historico);
        }

        public bool AceitarTroca(Guid requestId)
        {
            var request = _tradeRepo.GetById(requestId);
            if (request == null || request.Status != "Pendente")
                return false;

            // Remove os livros da listagem
            _bookRepo.Delete(request.BookRequestedId);
            if (request.BookOfferedId.HasValue)
                _bookRepo.Delete(request.BookOfferedId.Value);

            // Atualiza o status original
            _tradeRepo.UpdateStatus(requestId, "Aceita");

            // Histórico para o dono do livro
            RegistrarHistorico(request, request.OwnerId, "enviado para troca");

            // Histórico para o solicitante
            RegistrarHistorico(request, request.RequesterId, "recebido por troca");

            return true;
        }

        public bool RecusarTroca(Guid requestId)
        {
            var request = _tradeRepo.GetById(requestId);
            if (request == null || request.Status != "Pendente")
                return false;

            // Atualiza o status original
            _tradeRepo.UpdateStatus(requestId, "Recusada");

            // Histórico para o dono do livro
            RegistrarHistorico(request, request.OwnerId, "recusado para troca");

            return true;
        }

        // Solicitações recebidas por um usuário
        public List<TradeRequest> ListarSolicitacoesRecebidas(Guid ownerId)
        {
            return _tradeRepo.GetByOwner(ownerId)
                             .Where(r => r.Status == "Pendente")
                             .ToList();
        }

        // Solicitações enviadas por um usuário
        public List<TradeRequest> ListarSolicitacoesEnviadas(Guid requesterId)
        {
            return _tradeRepo.GetByRequester(requesterId)
                             .OrderByDescending(r => r.CreatedAt)
                             .ToList();
        }
    }
}