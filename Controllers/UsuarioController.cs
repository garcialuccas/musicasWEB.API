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
    public class UsuarioController : ControllerBase
    {
        private readonly IGenericsRepository<Usuario> _context;
        public UsuarioController(IGenericsRepository<Usuario> context)
        {
            _context = context;
        }


        [HttpGet("ListaUsuarios")]
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

        [HttpPost("CadastrarUsuario")]
        public async Task<IActionResult> postAlbum([FromForm] UsuarioDTO albumDTO)
        {
            try
            {
                Usuario avaliacao = new Usuario
                {
                    Nome = albumDTO.Nome!,
                    Email = albumDTO.email!,
                    Senha = albumDTO.senha!,
                    TipoUsuario = albumDTO.tipoUsuario!,
                    Ativo = true
                };

                await _context.cadastrar(avaliacao);
                return StatusCode(201, avaliacao);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("AtualizarUsuario/{id:guid}")]
        public async Task<IActionResult> putAlbum(Guid id, [FromForm] UsuarioDTO albumDTO)
        {
            try
            {
                var albumExistente = await _context.buscarPorId(id);
                if (albumExistente == null)
                    return NotFound(new { mensagem = "Usuario não encontrado." });

                if (!string.IsNullOrEmpty(albumDTO.Nome))
                    albumExistente.Nome = albumDTO.Nome;

                if (!string.IsNullOrEmpty(albumDTO.email))
                    albumExistente.Email = albumDTO.email;

                if (!string.IsNullOrEmpty(albumDTO.senha))
                    albumExistente.Senha = albumDTO.senha;

                if (!string.IsNullOrEmpty(albumDTO.tipoUsuario))
                    albumExistente.TipoUsuario = albumDTO.tipoUsuario;

                await _context.atualizar(id, albumExistente);
                return Ok(new { mensagem = "Usuario atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.InnerException?.Message ?? ex.Message });
            }
        }


        [HttpDelete("DeletarUsuario/{id:guid}")]
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