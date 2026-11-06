using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace musicasWEB.API.Models;

public partial class Musica
{
    [Key]
    [Column("idMusica")]
    public Guid IdMusica { get; set; }

    [Column("nome")]
    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column("duracao")]
    public int Duracao { get; set; }

    [Column("letra")]
    [Unicode(false)]
    public string? Letra { get; set; }

    [Column("imagemCapa")]
    [Unicode(false)]
    public string? ImagemCapa { get; set; }

    [Column("dataLancamento")]
    public DateOnly? DataLancamento { get; set; }

    [Column("idAlbum")]
    public Guid? IdAlbum { get; set; }

    [Column("idArtista")]
    public Guid IdArtista { get; set; }

    [Column("idGenero")]
    public Guid? IdGenero { get; set; }

    [InverseProperty("IdMusicaNavigation")]
    public virtual ICollection<Avaliacao> Avaliacao { get; set; } = new List<Avaliacao>();

    [ForeignKey("IdAlbum")]
    [InverseProperty("Musica")]
    public virtual Album? IdAlbumNavigation { get; set; }

    [ForeignKey("IdArtista")]
    [InverseProperty("Musica")]
    public virtual Artista IdArtistaNavigation { get; set; } = null!;

    [ForeignKey("IdGenero")]
    [InverseProperty("Musica")]
    public virtual Genero? IdGeneroNavigation { get; set; }
}
