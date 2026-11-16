using System.ComponentModel.DataAnnotations;

namespace musicasWEB.API.DTO
{
    public class AlbumDTO : IValidatableObject
    {
        [StringLength(100)]
        public string? Nome { get; set; }

        public int? DuracaoTotal { get; set; }

        public DateOnly? DataLancamento { get; set; }

        public Guid? IdArtista { get; set; }

        public string? ImagemCapa { get; set; }

        public IFormFile? ArquivoImagem { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Regra que vale tanto para POST quanto para PUT
            if (ArquivoImagem is not null && !string.IsNullOrWhiteSpace(ImagemCapa))
            {
                yield return new ValidationResult(
                    "Envie o Arquivo Imagem ou a ImagemCapa, não os dois",
                    new[] { nameof(ArquivoImagem), nameof(ImagemCapa) }
                );
            }

            // Descobre se a requisição atual é um POST (cadastro)
            var httpContextAccessor = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            var isPost = httpContextAccessor?.HttpContext?.Request.Method == HttpMethods.Post;

            if (isPost)
            {
                if (string.IsNullOrWhiteSpace(Nome))
                    yield return new ValidationResult("O nome do álbum é obrigatório no cadastro.", new[] { nameof(Nome) });

                if (IdArtista is null || IdArtista == Guid.Empty)
                    yield return new ValidationResult("O IdArtista é obrigatório no cadastro.", new[] { nameof(IdArtista) });
            }
        }
    }
}