using System.ComponentModel.DataAnnotations;

namespace musicasWEB.API.DTOs
{
    public class ArtistaDTO : IValidatableObject 
    {
        public string? Nome { get; set; }
        public string? Biografia { get; set; }
        public string? ImagemPerfil { get; set; } // Ou IFormFile? se estiver recebendo o arquivo direto

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var httpContextAccessor = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            var isPost = httpContextAccessor?.HttpContext?.Request.Method == HttpMethods.Post;

            if (isPost)
            {
                if (string.IsNullOrWhiteSpace(Nome))
                    yield return new ValidationResult("O nome do artista é obrigatório no cadastro.", new[] { nameof(Nome) });
            }

            if (!string.IsNullOrWhiteSpace(Nome) && Nome.Length > 100)
                yield return new ValidationResult("O nome do artista deve ter no máximo 100 caracteres.", new[] { nameof(Nome) });
        }
    }
}