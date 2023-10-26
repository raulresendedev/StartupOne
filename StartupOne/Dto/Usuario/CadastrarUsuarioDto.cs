using StartupOne.Models;
using System.ComponentModel.DataAnnotations;

namespace StartupOne.Dto.Usuario
{
    public class CadastrarUsuarioDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Passwordconfirm { get; set; }

    }

}
