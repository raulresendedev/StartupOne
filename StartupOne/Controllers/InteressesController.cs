using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StartupOne.Dto.Interesses;
using StartupOne.Models;
using StartupOne.Service;
using StartupOne.Utils;

namespace StartupOne.Controllers
{
    [Route("api/[controller]")]
    public class InteressesController : Controller
    {
        private InteressesService _interessesService;

        public InteressesController(InteressesService eventosPendentesService)
        {
            _interessesService = eventosPendentesService;
        }

        [HttpPost("cadastrar")]
        [Authorize]
        public IActionResult CadastrarInteresse([FromBody] InteressesDto interesseDto)
        {
            try
            {
                return Ok(_interessesService.CadastrarInteresse(interesseDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("buscar-interesses-do-usuario/{idUsuario}")]
        [Authorize]
        public IActionResult ObterTodosEventos([FromRoute] int idUsuario)
        {
            try
            {
                var interesses = _interessesService.ObterInteressesDoUsuario(idUsuario);
                return Ok(interesses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("buscar-evento/{idUsuario}")]
        [Authorize]
        public IActionResult ObterEvento([FromRoute] int idUsuario)
        {
            try
            {
                var eventosUsuario = _interessesService.ObterEvento(idUsuario);
                return Ok(eventosUsuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deletar-interesse/{idInteresse}")]
        [Authorize]
        public IActionResult DeletarInteresse([FromRoute] int idInteresse)
        {
            {
                try
                {
                    _interessesService.DeletarInteresse(idInteresse);
                    return Ok("Interesse deletado com sucesso!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("atualizar-interesse")]
        [Authorize]
        public IActionResult AtualizarInteresse([FromBody] InteressesDto interessesDto)
        {
            {
                try
                {
                    _interessesService.AtualizarInteresse(interessesDto);
                    return Ok(interessesDto);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("encontrar-horarios/{idUsuario}")]
        [Authorize]
        public IActionResult EncontrarHorarios([FromRoute] int idUsuario)
        {
            {
                try
                {
                    string result = _interessesService.EncontrarHorarios(idUsuario);
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
