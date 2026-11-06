using Microsoft.EntityFrameworkCore;
using musicasWEB.API.Interfaces;

namespace musicasWEB.API.Repositories
{
    public class GenericsRepository<T> : IGenericsRepository<T> where T : class
    {

        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericsRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task atualizar(Guid id, T entity)
        {
            var objetoBanco = _dbSet.FindAsync(id);

            if (objetoBanco == null) throw new Exception("Objeto não encontrado");

            _context.Entry(entity).CurrentValues.SetValues(objetoBanco);
            await _context.SaveChangesAsync();
        }

        public async Task<T> buscarPorId(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task cadastrar(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task deletar(Guid id)
        {
            _dbSet.Remove(await _dbSet.FindAsync(id));
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> listar()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
