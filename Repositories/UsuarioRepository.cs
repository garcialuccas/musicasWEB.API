using Microsoft.EntityFrameworkCore;
using musicasWEB.API.Data;
using musicasWEB.API.Interfaces;
using musicasWEB.API.Models;
using musicasWEB.API.Utils;

namespace musicasWEB.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly MusicaContext _context;

        public UsuarioRepository(MusicaContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> buscarPorEmailSenha(string email, string senha)
        {
            var u = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);

            if (u is null) return null;

            if (!Criptografia.CompararHash(senha, u.Senha)) return null;

            return u;
        }
    }
}
