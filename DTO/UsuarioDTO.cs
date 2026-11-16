using System.ComponentModel.DataAnnotations;

namespace musicasWEB.API.DTO
{
    public class UsuarioDTO : IValidatableObject
    {
        [StringLength(100, ErrorMessage = "O nome deve conter no máximo 100 caracteres.")]
        public string? Nome { get; set; }

        [StringLength(100, ErrorMessage = "O email deve conter no máximo 100 caracteres.")]
        public string? email { get; set; }

        public string? senha { get; set; }

        [StringLength(20, ErrorMessage = "O tipo de usuário deve conter no máximo 20 caracteres.")]
        public string? tipoUsuario { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var httpContextAccessor = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            var isPost = httpContextAccessor?.HttpContext?.Request.Method == HttpMethods.Post;

            if (isPost)
            {
                if (string.IsNullOrWhiteSpace(Nome))
                    yield return new ValidationResult("O nome é obrigatório no cadastro.", new[] { nameof(Nome) });

                if (string.IsNullOrWhiteSpace(email))
                    yield return new ValidationResult("O email é obrigatório no cadastro.", new[] { nameof(email) });

                if (string.IsNullOrWhiteSpace(senha))
                    yield return new ValidationResult("A senha é obrigatória no cadastro.", new[] { nameof(senha) });

                if (string.IsNullOrWhiteSpace(tipoUsuario))
                    yield return new ValidationResult("O tipo de usuário é obrigatório no cadastro.", new[] { nameof(tipoUsuario) });
            }
        }
    }
}