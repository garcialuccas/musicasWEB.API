using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using musicasWEB.API.DTO;
using musicasWEB.API.DTOs;
using musicasWEB.API.Interfaces;
using musicasWEB.API.Models;

namespace musicasWEB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistaController : ControllerBase
    {
        private readonly IGenericsRepository<Artista> _context;
        public ArtistaController(IGenericsRepository<Artista> context)
        {
            _context = context;
        }


        [HttpGet("ListaArtista")]
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

        [HttpPost("CadastrarArtista")]
        public async Task<IActionResult> postAlbum([FromForm] ArtistaDTO albumDTO)
        {
            try
            {
                Artista album = new Artista
                {
                    Nome = albumDTO.Nome!,
                    Biografia = albumDTO.Biografia,
                    ImagemPerfil = albumDTO.ImagemPerfil
                };

                await _context.cadastrar(album);
                return StatusCode(201, album);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("AtualizarArtista/{id:guid}")]
        public async Task<IActionResult> putAlbum(Guid id, [FromForm] ArtistaDTO albumDTO)
        {
            try
            {
                var albumExistente = await _context.buscarPorId(id);
                if (albumExistente == null)
                    return NotFound(new { mensagem = "Artista não encontrado." });

                if (!string.IsNullOrWhiteSpace(albumDTO.Nome))
                    albumExistente.Nome = albumDTO.Nome;

                if (!string.IsNullOrEmpty(albumDTO.Biografia))
                    albumExistente.Biografia = albumDTO.Biografia;

                if (string.IsNullOrEmpty(albumDTO.ImagemPerfil))
                    albumExistente.ImagemPerfil = albumDTO.ImagemPerfil;

                await _context.atualizar(id, albumExistente);
                return Ok(new { mensagem = "Artista atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.InnerException?.Message ?? ex.Message });
            }
        }


        [HttpDelete("DeletarArtista/{id:guid}")]
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