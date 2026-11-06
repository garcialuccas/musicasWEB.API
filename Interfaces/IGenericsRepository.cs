namespace musicasWEB.API.Interfaces
{
    public interface IGenericsRepository<T> where T : class
    {
        Task cadastrar(T entity);
        Task<IEnumerable<T>> listar();

        Task<T> buscarPorId(Guid id);

        Task atualizar(Guid id, T entity);

        Task deletar(Guid id);
    }
}
