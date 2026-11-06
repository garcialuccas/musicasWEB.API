using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using musicasWEB.API.Models;

namespace musicasWEB.API.BdContextEvent;

public partial class EventContext : DbContext
{
    public EventContext()
    {
    }

    public EventContext(DbContextOptions<EventContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Album { get; set; }

    public virtual DbSet<Artista> Artista { get; set; }

    public virtual DbSet<Avaliacao> Avaliacao { get; set; }

    public virtual DbSet<Genero> Genero { get; set; }

    public virtual DbSet<Musica> Musica { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=D03S22-1252910\\MSSQLSERVER2;Database=MusicasDB;User=sa;Password=Senai@134;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.IdAlbum).HasName("PK__Album__F01A07484683F6C1");

            entity.Property(e => e.IdAlbum).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Album).HasConstraintName("FK__Album__idArtista__59063A47");
        });

        modelBuilder.Entity<Artista>(entity =>
        {
            entity.HasKey(e => e.IdArtista).HasName("PK__Artista__DC480292A3947B4C");

            entity.Property(e => e.IdArtista).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Avaliacao>(entity =>
        {
            entity.HasKey(e => e.IdAvaliacao).HasName("PK__Avaliaca__2A0C831202C9E735");

            entity.Property(e => e.IdAvaliacao).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdMusicaNavigation).WithMany(p => p.Avaliacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Avaliacao__idMus__6477ECF3");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Avaliacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Avaliacao__idUsu__6383C8BA");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero).HasName("PK__Genero__85223DA39029380B");

            entity.Property(e => e.IdGenero).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Musica>(entity =>
        {
            entity.HasKey(e => e.IdMusica).HasName("PK__Musica__41A84A85D9D75FED");

            entity.Property(e => e.IdMusica).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdAlbumNavigation).WithMany(p => p.Musica).HasConstraintName("FK__Musica__idAlbum__5DCAEF64");

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Musica)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Musica__idArtist__5EBF139D");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.Musica).HasConstraintName("FK__Musica__idGenero__5FB337D6");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__645723A61158619B");

            entity.ToTable(tb => tb.HasTrigger("TR_UsuarioSoftDelete"));

            entity.Property(e => e.IdUsuario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Ativo).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
