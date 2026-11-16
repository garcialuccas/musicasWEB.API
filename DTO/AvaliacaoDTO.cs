using System.ComponentModel.DataAnnotations;

namespace musicasWEB.API.DTOs
{
    public class AvaliacaoDTO : IValidatableObject
    {
        public int? Nota { get; set; }
        public string? Comentario { get; set; }
        public Guid? IdMusica { get; set; }
        public Guid? IdUsuario { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var httpContextAccessor = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            var isPost = httpContextAccessor?.HttpContext?.Request.Method == HttpMethods.Post;

            if (isPost)
            {
                if (Nota is null)
                    yield return new ValidationResult("A nota é obrigatória no cadastro.", new[] { nameof(Nota) });

                if (IdMusica is null || IdMusica == Guid.Empty)
                    yield return new ValidationResult("O IdMusica é obrigatório no cadastro.", new[] { nameof(IdMusica) });

                if (IdUsuario is null || IdUsuario == Guid.Empty)
                    yield return new ValidationResult("O IdUsuario é obrigatório no cadastro.", new[] { nameof(IdUsuario) });
            }

            if (Nota.HasValue && (Nota < 1 || Nota > 5))
                yield return new ValidationResult("A nota deve ser entre 1 e 5.", new[] { nameof(Nota) });
        }
    }
}