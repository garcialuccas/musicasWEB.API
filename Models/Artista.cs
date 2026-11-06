using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace musicasWEB.API.Models;

public partial class Artista
{
    [Key]
    [Column("idArtista")]
    public Guid IdArtista { get; set; }

    [Column("nome")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column("biografia")]
    [Unicode(false)]
    public string? Biografia { get; set; }

    [Column("imagemPerfil")]
    [Unicode(false)]
    public string? ImagemPerfil { get; set; }

    [InverseProperty("IdArtistaNavigation")]
    public virtual ICollection<Album> Album { get; set; } = new List<Album>();

    [InverseProperty("IdArtistaNavigation")]
    public virtual ICollection<Musica> Musica { get; set; } = new List<Musica>();
}
