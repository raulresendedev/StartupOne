using Microsoft.AspNetCore.Mvc;
using StartupOne.Dto.Usuario;
using StartupOne.Service;
using StartupOne.Utils;

namespace StartupOne.Controllers
{
    //https://localhost:7148/api/Usuario
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private UsuarioService _usuarioService = new UsuarioService();

        [HttpPost("cadastrar")]
        public IActionResult Index([FromBody] CadastrarUsuarioDto usuario)
        {
            try
            {
                _usuarioService.CadastrarUsuario(usuario);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
