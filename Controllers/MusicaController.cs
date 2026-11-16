using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using musicasWEB.API.DTO;
using musicasWEB.API.Interfaces;
using musicasWEB.API.Models;
using musicasWEB.API.Repositories;
using musicasWEB.API.Services;

namespace musicasWEB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicaController : ControllerBase
    {
        private readonly IGenericsRepository<Musica> _repository;
        private readonly IImagemService _imageService;

        public MusicaController(IGenericsRepository<Musica> repository, IImagemService imageService)
        {
            _repository = repository;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> cadastrar([FromForm] MusicaDTO dto)
        {
            if (dto.ArquivoImagem is not null && !_imageService.IsConfigured)
            {
                return Problem("Configure as credenciais do Cloudinary para enviar imagens.", statusCode: 503);
            }

            string? novaUrl = null;

            try
            {
                if (dto.ArquivoImagem is not null)
                {
                    novaUrl = await _imageService.UploadImageAsync(dto.ArquivoImagem, ImageFolder.Musicas);
                }
            }
            catch (ArgumentException erro) { return BadRequest(erro.Message); }
            catch (HttpRequestException) { return Problem("Falha ao enviar foto ao Clodinary.", statusCode: 502); }

            var m = new Musica 
            {
                Nome = dto.Nome,
                Duracao = dto.Duracao,
                Letra = dto.Letra,
                IdGenero = dto.IdGenero,
                DataLancamento = dto.DataLancamento,
                IdAlbum = dto.IdAlbum,
                IdArtista = dto.IdArtista,
                ImagemCapa = novaUrl ?? (string.IsNullOrWhiteSpace(dto.ImagemCapa) ? null : dto.ImagemCapa)
            };


            try
            {
                await _repository.cadastrar(m);
            }
            catch
            {
                if (novaUrl is not null) await _imageService.DeleteImageAsync(novaUrl);
                throw;
            }

            return CreatedAtAction(nameof(buscarPorId), new { id = m.IdMusica }, MusicaConverterDTO.Converter(m));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> buscarPorId(Guid id) 
        {
            var m = await _repository.buscarPorId(id);
            return m is null ? NotFound() : Ok(m);
        }

        [HttpGet]
        public async Task<IActionResult> listarPaginado([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10, [FromQuery] string? busca = null)
        {
            var (items, total) = await _repository.listarPaginado(pagina, tamanhoPagina, busca);
            return Ok(new { items, total, pagina, tamanhoPagina });
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> atualizar(Guid id, [FromForm] MusicaDTO dto)
        {
            var atual = await _repository.buscarPorId(id);
            if (atual == null) return NotFound();

            if (dto.ArquivoImagem is not null && !_imageService.IsConfigured) return Problem("Configure as credenciais do Cloudinary para enviar imagens.", statusCode: 503);

            string? novaUrl = null;

            try
            {
                if (dto.ArquivoImagem is not null)
                {
                    novaUrl = await _imageService.UploadImageAsync(dto.ArquivoImagem, ImageFolder.Musicas);
                }
            }
            catch (ArgumentException erro) { return BadRequest(erro.Message); }
            catch (HttpRequestException) { return Problem("Falha ao enviar a capa ao Cloudinary.", statusCode: 502); }

            var imagem = novaUrl ?? (string.IsNullOrWhiteSpace(dto.ImagemCapa) ? atual.ImagemCapa : dto.ImagemCapa);

            try
            {
                await _repository.atualizar(id, new Musica
                {
                    Nome = dto.Nome,
                    Duracao = dto.Duracao,
                    Letra = dto.Letra,
                    IdGenero = dto.IdGenero,
                    DataLancamento = dto.DataLancamento,
                    IdAlbum = dto.IdAlbum,
                    IdArtista = dto.IdArtista,
                    ImagemCapa = novaUrl ?? (string.IsNullOrWhiteSpace(dto.ImagemCapa) ? null : dto.ImagemCapa)
                });
            }
            catch
            {
                if (novaUrl is not null) await _imageService.DeleteImageAsync(novaUrl);
                throw;
            }

            if (!string.IsNullOrWhiteSpace(atual.ImagemCapa) && atual.ImagemCapa != imagem) await _imageService.DeleteImageAsync(atual.ImagemCapa);

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> excluir(Guid id)
        {
            var m = await _repository.buscarPorId(id);

            if (m is null) return NotFound();

            await _repository.deletar(id);
            if (!string.IsNullOrWhiteSpace(m.ImagemCapa)) await _imageService.DeleteImageAsync(m.ImagemCapa);
            return NoContent();
        }
    }
}
