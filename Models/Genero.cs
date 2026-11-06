using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace musicasWEB.API.Models;

[Index("Nome", Name = "UQ__Genero__6F71C0DC01E75556", IsUnique = true)]
public partial class Genero
{
    [Key]
    [Column("idGenero")]
    public Guid IdGenero { get; set; }

    [Column("nome")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [InverseProperty("IdGeneroNavigation")]
    public virtual ICollection<Musica> Musica { get; set; } = new List<Musica>();
}
