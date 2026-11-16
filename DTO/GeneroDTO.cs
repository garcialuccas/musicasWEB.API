using System.ComponentModel.DataAnnotations;

namespace musicasWEB.API.DTO
{
    public class GeneroDTO
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;
    }
}
