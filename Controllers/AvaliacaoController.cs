using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using musicasWEB.API.DTOs;
using musicasWEB.API.Interfaces;
using musicasWEB.API.Models;

namespace musicasWEB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IGenericsRepository<Avaliacao> _context;
        public AvaliacaoController(IGenericsRepository<Avaliacao> context)
        {
            _context = context;
        }


        [HttpGet("ListaAvaliacoes")]
        public async Task<IActionResult> lista()
        {
            try
            {
                return Ok(await _context.listar());
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPost("CadastrarAvaliacao")]
        public async Task<IActionResult> postAlbum([FromForm] AvaliacaoDTO albumDTO)
        {
            try
            {
                Avaliacao avaliacao = new Avaliacao
                {
                    Nota = albumDTO.Nota!.Value,
                    Comentario = albumDTO.Comentario,
                    IdMusica = albumDTO.IdMusica!.Value,
                    IdUsuario = albumDTO.IdUsuario!.Value
                };

                await _context.cadastrar(avaliacao);
                return StatusCode(201, avaliacao);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("AtualizarAvaliacao/{id:guid}")]
        public async Task<IActionResult> putAlbum(Guid id, [FromForm] AvaliacaoDTO albumDTO)
        {
            try
            {
                var albumExistente = await _context.buscarPorId(id);
                if (albumExistente == null)
                    return NotFound(new { mensagem = "Avaliacao não encontrado." });

                if (albumDTO.IdMusica.HasValue)
                    albumExistente.IdMusica = albumDTO.IdMusica.Value;

                if (albumDTO.Nota.HasValue)
                    albumExistente.Nota = albumDTO.Nota.Value;

                if (string.IsNullOrEmpty(albumDTO.Comentario))
                    albumExistente.Comentario = albumDTO.Comentario;

                await _context.atualizar(id, albumExistente);
                return Ok(new { mensagem = "Avaliacao atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.InnerException?.Message ?? ex.Message });
            }
        }


        [HttpDelete("DeletarAvaliacao/{id:guid}")]
        public async Task<IActionResult> deletarAlbum(Guid id)
        {
            try
            {
                var buscarPorId = await _context.buscarPorId(id);

                if (buscarPorId == null)
                {
                    return StatusCode(404);
                }

                await _context.deletar(id);

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }

}
