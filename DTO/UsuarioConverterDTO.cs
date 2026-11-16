using Microsoft.Extensions.Primitives;
using musicasWEB.API.Models;

namespace Plot.WebAPI.DTO
{
    public record UsuarioConverterDTO(Guid idUsuario, string nome, string email, string tipoUsuario)
    {
        public static UsuarioConverterDTO Converter(Usuario u) => new(u.IdUsuario, u.Nome, u.Email, u.TipoUsuario);
    }
}
