using StartupOne.Mapping;
using StartupOne.Models;
using StartupOne.Repository;

namespace StartupOne.Service
{
    public class EventosMarcadosService
    {
        private readonly EventosMarcadosRepository _eventosRepository = new();

        public void CadastrarEvento(EventosMarcados evento)
        {
            _eventosRepository.Adicionar(evento);
        }

        public void AtualizarEvento(EventosMarcados evento)
        {
            _eventosRepository.Atualizar(evento);
        }

        public void DeletarEvento(int idEvento)
        {
            var evento = _eventosRepository.Obter(idEvento);
            _eventosRepository.Remover(evento);
        }

        public EventosMarcados ObterEvento(int idEvento)
        {
            return _eventosRepository.Obter(idEvento);
        }

        public ICollection<EventosMarcados> ObterTodosEventos(int idUsuario)
        {
            return _eventosRepository.ObterEventosMarcadosDoUsuario(idUsuario);
        }

    }
}
