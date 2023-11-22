using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using StartupOne.Dto.Usuario;
using StartupOne.Exceptions;
using StartupOne.Service;
using StartupOne.Utils;

namespace StartupOne.Controllers
{

    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("cadastrar")]
        public IActionResult Index([FromBody] CadastrarUsuarioDto usuario)
        {
            try
            {
                UsuarioLogadoDto usuarioLogado = _usuarioService.CadastrarUsuario(usuario);
                return Ok(usuarioLogado);
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException is OracleException oracleEx && oracleEx.Number == 1 && oracleEx.Message.Contains("UQ_STO_USUARIO_EMAIL"))
            {
                return BadRequest(new EmailDuplicadoException().Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logar")]
        public IActionResult Index([FromBody] LoginUsuarioDto usuario)
        {
            try
            {
                UsuarioLogadoDto usuarioLogado = _usuarioService.LogarUsuario(usuario);
                return Ok(usuarioLogado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
