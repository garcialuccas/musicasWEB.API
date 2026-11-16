using musicasWEB.API.Models;
using System.ComponentModel.DataAnnotations;

namespace musicasWEB.API.DTO
{
    public record MusicaConverterDTO(Guid IdMusica, string Nome, int Duracao, string? Letra, string? ImagemCapa, Guid? IdAlbum, Guid? IdGenero)
    {
        public static MusicaConverterDTO Converter(Musica m) => new(m.IdMusica, m.Nome, m.Duracao, m.Letra, m.ImagemCapa, m.IdAlbum, m.IdGenero);
    }
}