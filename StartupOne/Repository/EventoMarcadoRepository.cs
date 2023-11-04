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
        public ICollection<EventoMarcado> ObterTodosEventosDoUsuario(int idUsuario)
        {
            return _dbContext.Set<EventoMarcado>()
                .Where(e => e.IdUsuario == idUsuario)
                .OrderBy(e => e.Inicio)
                .ToList();
        }

        public ICollection<EventoMarcado> ObterEventosFuturosDoUsuario(int idUsuario)
        {
            return _dbContext.Set<EventoMarcado>()
                .Where(e => e.IdUsuario == idUsuario && e.Inicio >= DateTime.Today)
                .OrderBy(e => e.Inicio)
                .ToList();
        }

        public ICollection<EventoMarcado> ObterEventosAtrasadosDoUsuario(int idUsuario)
        {
            return _dbContext.Set<EventoMarcado>()
                .Where(e => e.IdUsuario == idUsuario && e.Inicio < DateTime.Today && !e.Concluido)
                .OrderBy(e => e.Inicio)
                .ToList();
        }

        public ICollection<EventoMarcado> ObterEventosConcluidosDoUsuario(int idUsuario)
        {
            return _dbContext.Set<EventoMarcado>()
                .Where(e => e.IdUsuario == idUsuario && e.Concluido)
                .OrderBy(e => e.Inicio)
                .ToList();
        }

        public bool JaExisteEventoNoPeriodo(EventoMarcado evento)
        {
            var conflitantes = _dbContext.Set<EventoMarcado>()
                            .Where(x => x.IdUsuario == evento.IdUsuario && x.IdEventoMarcado != evento.IdEventoMarcado
                                        && ((evento.Inicio >= x.Inicio && evento.Inicio <= x.Fim) ||
                                            (evento.Fim >= x.Inicio && evento.Fim <= x.Fim))).Count();

            return conflitantes != 0 ? true : false;
        }
    }
}
