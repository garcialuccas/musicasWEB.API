using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace musicasWEB.API.Models;

[Index("Email", Name = "UQ__Usuario__AB6E61648158B414", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("idUsuario")]
    public Guid IdUsuario { get; set; }

    [Column("nome")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("senha")]
    [StringLength(255)]
    [Unicode(false)]
    public string Senha { get; set; } = null!;

    [Column("tipoUsuario")]
    [StringLength(20)]
    [Unicode(false)]
    public string TipoUsuario { get; set; } = null!;

    [Column("ativo")]
    public bool Ativo { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Avaliacao> Avaliacao { get; set; } = new List<Avaliacao>();
}
