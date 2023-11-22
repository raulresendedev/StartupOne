
using StartupOne.Models;
using System.Data.Entity;

namespace StartupOne.Repository
{
    public class InteressesRepository : BaseRepository<Interesses>
    {
        public ICollection<Interesses> ObterInteressesDoUsuario(int idUsuario)
        {
            return _dbContext.Set<Interesses>()
                .Where(e => e.IdUsuario == idUsuario)
                .OrderByDescending(x => x.Prioridade)
                .ToList();
        }
    }
}
