using musicasWEB.API.Models;

namespace musicasWEB.API.Interfaces
{
    public interface IGenericsRepository<T> where T : class
    {
        Task cadastrar(T entity);
        Task<IEnumerable<T>> listar();

        Task<T> buscarPorId(Guid id);

        Task atualizar(Guid id, T entity);

        Task deletar(Guid id);

        Task<Album> ListaDeMusica(Guid id);
        Task<(List<T> lista, int total)> listarPaginado(int pagina = 1, int tamanhoPagina = 10, string? busca = null);
    }
}