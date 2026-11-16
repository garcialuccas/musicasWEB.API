using System.ComponentModel.DataAnnotations;

namespace musicasWEB.API.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O Email é um campo obrigatorio.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A Senha é um campo obrigatorio.")]
        public string Senha { get; set; }
    }
}