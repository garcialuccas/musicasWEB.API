use master;
go

-- Se o banco já existir, remove para recriar limpo (Opcional, remova se preferir manter)
if db_id('MusicasDB') is not null
begin
    alter database MusicasDB set single_user with rollback immediate;
    drop database MusicasDB;
end
go

create database MusicasDB;
GO

use MusicasDB;
go

-- 1. Criação das Tabelas
create table Usuario (
    idUsuario UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    nome varchar(100) not null,
    email VARCHAR(100) not null unique,
    senha VARCHAR(255) not null,
    tipoUsuario varchar(20) not null check (tipoUsuario in ('admin', 'usuario')),
    ativo bit default 1 not null
);
go

create table Genero (
    idGenero UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
    nome varchar(100) not null unique
);
GO

create table Artista (
    idArtista UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    nome varchar(100) not null,
    biografia varchar(MAX),
    imagemPerfil varchar(MAX)
);
GO

create table Album (
    idAlbum UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    nome varchar(100) not null,
    duracaoTotal int not null check (duracaoTotal > 0), -- em minutos
    dataLancamento DATE,
    imagemCapa varchar(MAX),
    idArtista UNIQUEIDENTIFIER REFERENCES Artista(idArtista)
);
go

create table Musica (
    idMusica UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
    nome varchar(100) not null,
    duracao int not null check (duracao > 0), -- em segundos
    letra varchar(MAX),
    imagemCapa varchar(MAX),
    dataLancamento DATE,

    idAlbum UNIQUEIDENTIFIER REFERENCES Album(idAlbum),
    idArtista UNIQUEIDENTIFIER references Artista(idArtista) NOT NULL,
    idGenero UNIQUEIDENTIFIER REFERENCES Genero(idGenero)
);
go

create table Avaliacao (
    idAvaliacao UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
    idUsuario UNIQUEIDENTIFIER REFERENCES Usuario(idUsuario) not null,
    idMusica UNIQUEIDENTIFIER REFERENCES Musica(idMusica) not null,
    nota float not null check (nota >= 0 AND nota <= 5), -- numero entre 0 e 5
    comentario varchar(MAX)
);
GO 

-- 2. Trigger de Soft Delete
CREATE TRIGGER TR_UsuarioSoftDelete
ON Usuario
INSTEAD OF DELETE
AS
BEGIN
    UPDATE Usuario
    SET Ativo = 0
    WHERE IdUsuario IN 
        (SELECT IdUsuario FROM deleted) 
END;
GO

-- 3. Inserções de Dados
insert into Usuario (nome, email, senha, tipoUsuario)
VALUES
('Laura', 'laura@email.com', convert(varchar(255), HASHBYTES('SHA2_256', 'lauralinda5533'), 2), 'admin'),
('Luccas', 'luccas@email.com', convert(varchar(255), HASHBYTES('SHA2_256', 'luccasgamestop'), 2), 'usuario');
GO

insert into Genero (nome)
VALUES
('Sertanejo'),
('Rock'),
('Gospel'),
('Forró'),
('MPB');
GO

insert into Artista (nome, biografia, imagemPerfil)
VALUES
('Filho do Piseiro', 'Everton da Silva de Souza, amplamente conhecido como Filho do Piseiro...', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTC9R17w5388jIG8zU7LJMyiNlQI6Ipe6ND5JaZ_jeOQB9VgGqTHDGLzHw&s=10'),
('Get Worship', 'O Get Worship é o ministério oficial de louvor da igreja Get Church Global...', 'https://media.hugogloss.uol.com.br/uploads/2026/04/Captura-de-Tela-2026-04-18-as-18.37.41.png'),
('Marisa Monte', 'Marisa Monte (Marisa de Azevedo Monte) é uma cantora, compositora...', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTj1fkE8L5HBukFXgkANAPl-yxNBYPH9GYvHhq2xy8LZw&s=10');
GO

insert into Album (nome, duracaoTotal, dataLancamento, imagemCapa, idArtista)
VALUES
('Piseiro Bruto', 45, '2025-05-10', 'capa_piseiro.jpg', (SELECT idArtista FROM Artista WHERE nome = 'Filho do Piseiro')),
('Até que o Céu Inunde a Terra', 60, '2024-09-15', 'capa_getworship.jpg', (SELECT idArtista FROM Artista WHERE nome = 'Get Worship')),
('Portas', 40, '2021-07-01', 'capa_portas.jpg', (SELECT idArtista FROM Artista WHERE nome = 'Marisa Monte'));
GO

insert into Musica (nome, duracao, letra, imagemCapa, dataLancamento, idAlbum, idArtista, idGenero)
VALUES
('Boca de Médio Grave', 210, 'Letra da música do Piseiro...', 'capa_musica1.jpg', '2025-05-10', 
 (SELECT idAlbum FROM Album WHERE nome = 'Piseiro Bruto'), 
 (SELECT idArtista FROM Artista WHERE nome = 'Filho do Piseiro'), 
 (SELECT idGenero FROM Genero WHERE nome = 'Forró')),

('Tudo a Ver', 300, 'Letra da música de louvor...', 'capa_musica2.jpg', '2024-09-15', 
 (SELECT idAlbum FROM Album WHERE nome = 'Até que o Céu Inunde a Terra'), 
 (SELECT idArtista FROM Artista WHERE nome = 'Get Worship'), 
 (SELECT idGenero FROM Genero WHERE nome = 'Gospel')),

('Calma', 245, 'Você não tem que ser perfeita o tempo todo...', 'capa_musica3.jpg', '2021-07-01', 
 (SELECT idAlbum FROM Album WHERE nome = 'Portas'), 
 (SELECT idArtista FROM Artista WHERE nome = 'Marisa Monte'), 
 (SELECT idGenero FROM Genero WHERE nome = 'MPB'));
GO

insert into Avaliacao (idUsuario, idMusica, nota, comentario)
VALUES
((SELECT idUsuario FROM Usuario WHERE email = 'laura@email.com'), 
 (SELECT idMusica FROM Musica WHERE nome = 'Calma'), 
 5.0, 'Simplesmente maravilhosa, a voz da Marisa é perfeita!'),

((SELECT idUsuario FROM Usuario WHERE email = 'luccas@email.com'), 
 (SELECT idMusica FROM Musica WHERE nome = 'Boca de Médio Grave'), 
 4.5, 'Ritmo contagiante, muito bom para curtir!');
GO