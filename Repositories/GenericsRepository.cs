using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using musicasWEB.API.Data;
using musicasWEB.API.Interfaces;
using musicasWEB.API.Models;

namespace musicasWEB.API.Repositories
{
    public class Classe
    {
        [NotMapped]
        public virtual Guid Id { get; set; }

        [NotMapped]
        public virtual string? Nome { get; set; }
    }

    public class GenericsRepository<T> : IGenericsRepository<T> where T : Classe
    {
        private readonly MusicaContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericsRepository(MusicaContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task atualizar(Guid id, T entity)
        {
            var objetoBanco = await _dbSet.FindAsync(id);

            if (objetoBanco == null)
                throw new Exception("Objeto não encontrado.");

            entity.Id = id;

            _context.Entry(objetoBanco).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> buscarPorId(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Album> ListaDeMusica(Guid id)
        {
            var buscaId = await _context.Album.Include(x => x.Musica).FirstOrDefaultAsync(x => x.IdAlbum == id);

            if (buscaId == null)
            {
                return new Album();
            }

            return buscaId;
        }


        public async Task cadastrar(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task deletar(Guid id)
        {
            var objetoBanco = await _dbSet.FindAsync(id);

            if (objetoBanco == null)
                throw new Exception("Objeto não encontrado.");

            _dbSet.Remove(objetoBanco);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> listar()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<(List<T> lista, int total)> listarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1) tamanhoPagina = 10;

            var query = _dbSet.AsNoTracking().AsQueryable();
            var entityType = _context.Model.FindEntityType(typeof(T));
            var temNome = entityType?.FindProperty("Nome") != null;
            var pkName = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault()?.Name;

            if (!string.IsNullOrWhiteSpace(busca) && temNome)
            {
                query = query.Where(i => EF.Property<string>(i, "Nome").Contains(busca));
            }

            int total = await query.CountAsync();

            if (temNome && pkName != null)
            {
                query = query
                    .OrderBy(i => EF.Property<string>(i, "Nome"))
                    .ThenBy(i => EF.Property<Guid>(i, pkName));
            }
            else if (pkName != null)
            {
                query = query.OrderBy(i => EF.Property<Guid>(i, pkName));
            }

            var items = await query
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (items, total);
        }
    }
}