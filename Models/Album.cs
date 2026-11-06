using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace musicasWEB.API.Models;

public partial class Album
{
    [Key]
    [Column("idAlbum")]
    public Guid IdAlbum { get; set; }

    [Column("nome")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column("duracaoTotal")]
    public int DuracaoTotal { get; set; }

    [Column("dataLancamento")]
    public DateOnly? DataLancamento { get; set; }

    [Column("imagemCapa")]
    [Unicode(false)]
    public string? ImagemCapa { get; set; }

    [Column("idArtista")]
    public Guid? IdArtista { get; set; }

    [ForeignKey("IdArtista")]
    [InverseProperty("Album")]
    public virtual Artista? IdArtistaNavigation { get; set; }

    [InverseProperty("IdAlbumNavigation")]
    public virtual ICollection<Musica> Musica { get; set; } = new List<Musica>();
}
