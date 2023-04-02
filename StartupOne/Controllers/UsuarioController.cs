using Microsoft.AspNetCore.Mvc;
using StartupOne.Models;
using StartupOne.Service;
using StartupOne.Utils;

namespace StartupOne.Controllers
{
    //https://localhost:7148/api/Usuario/cadastrar
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private UsuarioService _usuarioService = new UsuarioService();

        [HttpPost("cadastrar")]
        [ValidateModel]
        public IActionResult Index([FromBody] Usuario usuario)
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
