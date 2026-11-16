using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace musicasWEB.API.DTO
{
    public class MusicaDTO : IValidatableObject
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int Duracao { get; set; }

        public string? Letra { get; set; }

        public string? ImagemCapa { get; set; }

        public IFormFile? ArquivoImagem { get; set; }

        public DateOnly? DataLancamento { get; set; }

        public Guid? IdAlbum { get; set; }

        public Guid IdArtista { get; set; }

        public Guid? IdGenero { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ArquivoImagem is not null && !string.IsNullOrWhiteSpace(ImagemCapa))
            {
                yield return new ValidationResult("Envie ArquivoImagem ou ImagemCapa, não os dois", new[] { nameof(ArquivoImagem), nameof(ImagemCapa) });
            }
            var httpContextAccessor = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            var isPost = httpContextAccessor?.HttpContext?.Request.Method == HttpMethods.Post;

            if (isPost)
            {
                if (string.IsNullOrWhiteSpace(Nome))
                    yield return new ValidationResult("O nome da música é obrigatória", new[] { nameof(Nome) });


                if (Duracao <= 0)
                    yield return new ValidationResult("A duracao deve ser posivitiva", new[] { nameof(Duracao) });


                if (IdArtista == null)
                    yield return new ValidationResult("O id do artista é obrigatório.", new[] { nameof(IdArtista) });

            }
        }
    }
}
