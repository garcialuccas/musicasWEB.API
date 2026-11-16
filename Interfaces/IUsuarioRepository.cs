using musicasWEB.API.Models;

namespace musicasWEB.API.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> buscarPorEmailSenha(string email, string senha);
    }
}
