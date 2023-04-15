using Microsoft.AspNetCore.Mvc;
using StartupOne.Models;
using StartupOne.Service;
using StartupOne.Utils;

namespace StartupOne.Controllers
{
    [Route("api/[controller]")]
    public class EventosPendentesController : Controller
    {
        private EventosPendentesService _eventosPendentesService = new EventosPendentesService();

        [HttpPost("cadastrar")]
        [ValidateModel]
        public IActionResult CadastrarEvento([FromBody] EventosPendentes evento)
        {
            try
            {
                _eventosPendentesService.CadastrarEvento(evento);
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
                var eventosUsuario = _eventosPendentesService.ObterTodosEventos(idUsuario);
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
                var eventosUsuario = _eventosPendentesService.ObterEvento(idUsuario);
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
                    _eventosPendentesService.DeletarEvento(idEvento);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPatch("atualizar-evento")]
        public IActionResult AtualizarEvento([FromBody] EventosPendentes evento)
        {
            {
                try
                {
                    _eventosPendentesService.AtualizarEvento(evento);
                    return Ok(evento);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("encontrar-horarios/{idUsuario}")]
        public IActionResult EncontrarHorarios([FromRoute] int idUsuario)
        {
            {
                try
                {
                    string result = _eventosPendentesService.EncontrarHorarios(idUsuario);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
