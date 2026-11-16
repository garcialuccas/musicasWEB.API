using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using musicasWEB.API.DTO;
using musicasWEB.API.Interfaces;
using musicasWEB.API.Models;

namespace musicasWEB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IGenericsRepository<Album> _context;
        public AlbumController(IGenericsRepository<Album> context)
        {
            _context = context;
        }


        [HttpGet("ListaAlbums")]
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

        [HttpGet("ListaMusicasPorAlbums/{id:guid}")]
        public async Task<IActionResult> listaAlbums(Guid id)
        {
            try
            {
                var buscaAlbums = await _context.ListaDeMusica(id);

                if (buscaAlbums == null)
                {
                    return NotFound();
                }

                var listaMusicas = buscaAlbums.Musica;


                return Ok(listaMusicas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }

        }

        [HttpPost("CadastrarAlbum")]
        public async Task<IActionResult> postAlbum([FromForm] AlbumDTO albumDTO)
        {
            try
            {
                Album album = new Album
                {
                    Nome = albumDTO.Nome!,
                    DuracaoTotal = albumDTO.DuracaoTotal ?? 0,
                    DataLancamento = albumDTO.DataLancamento,
                    ImagemCapa = albumDTO.ImagemCapa,
                    IdArtista = albumDTO.IdArtista!.Value
                };

                await _context.cadastrar(album);
                return StatusCode(201, album);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("AtualizarAlbum/{id:guid}")]
        public async Task<IActionResult> putAlbum(Guid id, [FromForm] AlbumDTO albumDTO)
        {
            try
            {
                var albumExistente = await _context.buscarPorId(id);
                if (albumExistente == null)
                    return NotFound(new { mensagem = "Álbum não encontrado." });

                if (!string.IsNullOrWhiteSpace(albumDTO.Nome))
                    albumExistente.Nome = albumDTO.Nome;

                if (albumDTO.DuracaoTotal.HasValue)
                    albumExistente.DuracaoTotal = albumDTO.DuracaoTotal.Value;

                if (albumDTO.DataLancamento.HasValue)
                    albumExistente.DataLancamento = albumDTO.DataLancamento.Value;

                if (!string.IsNullOrWhiteSpace(albumDTO.ImagemCapa))
                    albumExistente.ImagemCapa = albumDTO.ImagemCapa;

                if (albumDTO.IdArtista.HasValue && albumDTO.IdArtista != Guid.Empty)
                    albumExistente.IdArtista = albumDTO.IdArtista.Value;

                await _context.atualizar(id, albumExistente);
                return Ok(new { mensagem = "Álbum atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.InnerException?.Message ?? ex.Message });
            }
        }


        [HttpDelete("DeletarAlbum/{id:guid}")]
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