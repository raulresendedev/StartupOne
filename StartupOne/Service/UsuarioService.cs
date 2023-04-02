using StartupOne.Mapping;
using StartupOne.Models;
using StartupOne.Repository;

namespace StartupOne.Service
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository = new();

        public void CadastrarUsuario(Usuario usuario) { 
            _usuarioRepository.Adicionar(usuario);
        }
    }
}
