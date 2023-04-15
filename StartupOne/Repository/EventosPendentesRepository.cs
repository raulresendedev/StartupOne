
using StartupOne.Models;
using System.Data.Entity;

namespace StartupOne.Repository
{
    public class EventosPendentesRepository : BaseRepository<EventosPendentes>
    {
        public ICollection<EventosPendentes> ObterEventosPendentesDoUsuario(int idUsuario)
        {
            return _dbContext.Set<EventosPendentes>()
                .Where(e => e.IdUsuario == idUsuario)
                .ToList();
        }
    }
}
