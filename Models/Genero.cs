using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using musicasWEB.API.Repositories;

namespace musicasWEB.API.Models;

[Index("Nome", Name = "UQ__Genero__6F71C0DC01E75556", IsUnique = true)]
public partial class Genero : Classe
{
    [NotMapped]
    public new Guid Id
    {
        get => IdGenero;
        set => IdGenero = value;
    }



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