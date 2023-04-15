using StartupOne.Mapping;
using StartupOne.Models;
using StartupOne.Repository;

namespace StartupOne.Service
{
    public class EventosMarcadosService
    {
        private readonly EventosMarcadosRepository _eventosRepository = new();

        public void ValidarEventoMarcado(EventosMarcados eventoMarcado)
        {
            if (eventoMarcado.Fim < eventoMarcado.Inicio)
                throw new Exception("Data fim não pode ser menor do que a data início.");

            if(_eventosRepository.ConsultarEventosConflitantes(eventoMarcado))
                throw new Exception("Já existe evento neste periodo.");

            if(eventoMarcado.Status != true)
                eventoMarcado.Status = true;

            if(eventoMarcado.Inicio.Minute % 5 != 0 || eventoMarcado.Fim.Minute % 5 != 0)
                throw new Exception("Horario inválido.");
        }

        public void CadastrarEvento(EventosMarcados evento)
        {
            ValidarEventoMarcado(evento);
            
            _eventosRepository.Adicionar(evento);
        }

        public void AtualizarEvento(EventosMarcados evento)
        {
            ValidarEventoMarcado(evento);

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
