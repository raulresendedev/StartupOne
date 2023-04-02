using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StartupOne.Models;
using StartupOne.Service;
using StartupOne.Utils;

namespace StartupOne.Controllers
{

    [Route("api/[controller]")]
    public class EventosMarcadosController : Controller
    {
        private EventosMarcadosService _eventosService = new EventosMarcadosService();

        [HttpPost("cadastrar")]
        [ValidateModel]
        public IActionResult CadastrarEvento([FromBody] EventosMarcados evento)
        {
            try
            {
                _eventosService.CadastrarEvento(evento);
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("buscar-todos-por-usuario/{idUsuario}")]
        public IActionResult ObterTodosEventos([FromRoute] int idUsuario)
        {
            try
            {
                var eventosUsuario = _eventosService.ObterTodosEventos(idUsuario);
                return Ok(eventosUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("buscar-evento/{idUsuario}")]
        public IActionResult ObterEvento([FromRoute] int idUsuario)
        {
            try
            {
                var eventosUsuario = _eventosService.ObterEvento(idUsuario);
                return Ok(eventosUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deletar-evento/{idEvento}")]
        public IActionResult DeletarEvento([FromRoute] int idEvento)
        {
            {
                try
                {
                    _eventosService.DeletarEvento(idEvento);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPatch("atualizar-evento")]
        public IActionResult AtualizarEvento([FromBody] EventosMarcados evento)
        {
            {
                try
                {
                    _eventosService.AtualizarEvento(evento);
                    return Ok(evento);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}