using StartupOne.Models;
using StartupOne.Repository;

namespace StartupOne.Service
{
    public class EventosPendentesService
    {
        private readonly EventosPendentesRepository _eventosRepository = new();

        public void CadastrarEvento(EventosPendentes evento)
        {
            _eventosRepository.Adicionar(evento);
        }

        public void AtualizarEvento(EventosPendentes evento)
        {
            _eventosRepository.Atualizar(evento);
        }

        public void DeletarEvento(int idEvento)
        {
            var evento = _eventosRepository.Obter(idEvento);
            _eventosRepository.Remover(evento);
        }

        public EventosPendentes ObterEvento(int idEvento)
        {
            return _eventosRepository.Obter(idEvento);
        }

        public ICollection<EventosPendentes> ObterTodosEventos(int idUsuario)
        {
            return _eventosRepository.ObterEventosMarcadosDoUsuario(idUsuario);
        }

    }
}