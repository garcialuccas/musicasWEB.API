using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using musicasWEB.API.DTO;
using musicasWEB.API.Interfaces;
using musicasWEB.API.Models;

namespace musicasWEB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {

        private readonly IGenericsRepository<Genero> _repository;

        public GeneroController(IGenericsRepository<Genero> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> cadastrar([FromForm] GeneroDTO dto)
        {
            var g = new Genero { Nome = dto.Nome };
            try
            {
                await _repository.cadastrar(g);
            }
            catch (DbUpdateException erro) when (erro.GetBaseException() is SqlException { Number: 2601 or 2627 })
            {
                return Conflict("Já existe um gênero com esse nome.");
            }

            return CreatedAtAction(nameof(buscarPorId), new { id = g.IdGenero });
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> buscarPorId(Guid id)
        {
            var g = await _repository.buscarPorId(id);
            return g is null ? NotFound() : Ok(g);
        }

        [HttpGet]
        public async Task<IActionResult> listar() => Ok(await _repository.listar());

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> atualizar(Guid id, [FromForm] GeneroDTO dto)
        {
            var g = await _repository.buscarPorId(id);

            if (g is null) return NotFound();

            if (dto.Nome is null) return NoContent();

            try
            {
                _repository.atualizar(id, new Genero { Nome = dto.Nome });
            }
            catch (DbUpdateException erro) when (erro.GetBaseException() is SqlException { Number: 2601 or 2627 })
            {
                return Conflict("Já existe um gênero com esse nome.");
            }

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> excluir(Guid id)
        {
            if (await _repository.buscarPorId(id) is null) return NotFound();
            await _repository.deletar(id);
            return NoContent();
        }
    }
}
