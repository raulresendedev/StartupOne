
using StartupOne.Models;

namespace StartupOne.Repository
{
    public class EventosPendentesRepository : BaseRepository<EventosPendentes>
    {
        public ICollection<EventosPendentes> ObterEventosMarcadosDoUsuario(int idUsuario)
        {
            return _dbContext.Set<EventosPendentes>()
                .Where(e => e.IdUsuario == idUsuario)
                .ToList();
        }
    }
}
