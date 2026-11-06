using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace musicasWEB.API.Models;

public partial class Avaliacao
{
    [Key]
    [Column("idAvaliacao")]
    public Guid IdAvaliacao { get; set; }

    [Column("idUsuario")]
    public Guid IdUsuario { get; set; }

    [Column("idMusica")]
    public Guid IdMusica { get; set; }

    [Column("nota")]
    public double Nota { get; set; }

    [Column("comentario")]
    [Unicode(false)]
    public string? Comentario { get; set; }

    [ForeignKey("IdMusica")]
    [InverseProperty("Avaliacao")]
    public virtual Musica IdMusicaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("Avaliacao")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
