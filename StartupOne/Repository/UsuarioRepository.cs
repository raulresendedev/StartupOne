using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StartupOne.Mapping;
using StartupOne.Models;
using System.Data.Entity;

namespace StartupOne.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>
    {
        public Usuario ObterPorEmail(string email)
        {
            return _dbContext.Usuarios.SingleOrDefault(u => u.Email == email);
        }
    }
}
