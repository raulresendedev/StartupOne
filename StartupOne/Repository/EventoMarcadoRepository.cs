using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Oracle.ManagedDataAccess.Client;
using StartupOne.Mapping;
using StartupOne.Models;
using System.Data.Entity;

namespace StartupOne.Repository
{
    public class EventoMarcadoRepository : BaseRepository<EventoMarcado>
    {
        public ICollection<EventoMarcado> ObterEventosMarcadosDoUsuario(int idUsuario)
        {
            
            return _dbContext.Set<EventoMarcado>()
                .Where(e => e.IdUsuario == idUsuario)
                .ToList();
        }

        public bool ConsultarEventosConflitantes(EventoMarcado evento)
        {
            var conflitantes = _dbContext.Set<EventoMarcado>()
                .Where(x => x.IdUsuario == evento.IdUsuario &&
                            (evento.Inicio >= x.Inicio && evento.Inicio <= x.Fim && x.IdEventoMarcado != evento.IdEventoMarcado) ||
                             (evento.Fim >= x.Inicio && evento.Fim <= x.Fim && x.IdEventoMarcado != evento.IdEventoMarcado))
                .Count();

            return conflitantes != 0 ? true : false;
        }
    }
}
