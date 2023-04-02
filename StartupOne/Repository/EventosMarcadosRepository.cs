using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Oracle.ManagedDataAccess.Client;
using StartupOne.Mapping;
using StartupOne.Models;
using System.Data.Entity;

namespace StartupOne.Repository
{
    public class EventosMarcadosRepository : BaseRepository<EventosMarcados>
    {
        public ICollection<EventosMarcados> ObterEventosMarcadosDoUsuario(int idUsuario)
        {
            return _dbContext.Set<EventosMarcados>()
                .Where(e => e.IdUsuario == idUsuario)
                .ToList();
        }
    }
}
