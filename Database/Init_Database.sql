USE [master]
GO
/****** Object:  Database [DB_KBFORUM]    Script Date: 2023-04-01 10:46:08 ******/
CREATE DATABASE [DB_KBFORUM]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DB_KBFORUM', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DB_KBFORUM.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DB_KBFORUM_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DB_KBFORUM_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [DB_KBFORUM] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB_KBFORUM].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DB_KBFORUM] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_KBFORUM] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_KBFORUM] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DB_KBFORUM] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_KBFORUM] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET RECOVERY FULL 
GO
ALTER DATABASE [DB_KBFORUM] SET  MULTI_USER 
GO
ALTER DATABASE [DB_KBFORUM] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_KBFORUM] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB_KBFORUM] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB_KBFORUM] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DB_KBFORUM] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DB_KBFORUM] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'DB_KBFORUM', N'ON'
GO
ALTER DATABASE [DB_KBFORUM] SET QUERY_STORE = OFF
GO
USE [DB_KBFORUM]
GO
/****** Object:  Table [dbo].[TBAlerta]    Script Date: 2023-04-01 10:46:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBAlerta](
	[ID] [uniqueidentifier] NOT NULL,
	[ModoAlerta] [int] NOT NULL,
	[UsuarioID] [varchar](20) NOT NULL,
	[TopicoID] [uniqueidentifier] NOT NULL,
	[DataCriacao] [datetime] NOT NULL,
	[DataModificacao] [datetime] NULL,
	[UsuarioCriacao] [varchar](20) NOT NULL,
	[UsuarioModificacao] [varchar](20) NULL,
	[Atualizacao] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBComentario]    Script Date: 2023-04-01 10:46:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBComentario](
	[ID] [uniqueidentifier] NOT NULL,
	[Conteudo] [varchar](400) NOT NULL,
	[Status] [bit] NOT NULL,
	[TopicoID] [uniqueidentifier] NOT NULL,
	[UsuarioID] [varchar](20) NOT NULL,
	[DataCriacao] [datetime] NOT NULL,
	[DataModificacao] [datetime] NULL,
	[UsuarioCriacao] [varchar](20) NOT NULL,
	[UsuarioModificacao] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBGrupo]    Script Date: 2023-04-01 10:46:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBGrupo](
	[ID] [uniqueidentifier] NOT NULL,
	[Descricao] [varchar](20) NOT NULL,
	[Status] [bit] NOT NULL,
	[DataCriacao] [datetime] NOT NULL,
	[DataModificacao] [datetime] NULL,
	[UsuarioCriacao] [varchar](20) NOT NULL,
	[UsuarioModificacao] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBTag]    Script Date: 2023-04-01 10:46:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBTag](
	[ID] [uniqueidentifier] NOT NULL,
	[Descricao] [varchar](20) NOT NULL,
	[DataCriacao] [datetime] NOT NULL,
	[DataModificacao] [datetime] NULL,
	[UsuarioCriacao] [varchar](20) NOT NULL,
	[UsuarioModificacao] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBTopico]    Script Date: 2023-04-01 10:46:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBTopico](
	[ID] [uniqueidentifier] NOT NULL,
	[Titulo] [varchar](60) NOT NULL,
	[Conteudo] [text] NOT NULL,
	[TipoAcesso] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[UsuarioID] [varchar](20) NOT NULL,
	[DataCriacao] [datetime] NOT NULL,
	[DataModificacao] [datetime] NULL,
	[UsuarioCriacao] [varchar](20) NOT NULL,
	[UsuarioModificacao] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBTopicoTag]    Script Date: 2023-04-01 10:46:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBTopicoTag](
	[TagID] [uniqueidentifier] NOT NULL,
	[TopicoID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CompositeTopicoTag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC,
	[TopicoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBUsuario]    Script Date: 2023-04-01 10:46:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBUsuario](
	[Nome] [varchar](120) NOT NULL,
	[Email] [varchar](80) NOT NULL,
	[Login] [varchar](20) NOT NULL,
	[Senha] [varchar](120) NOT NULL,
	[Status] [bit] NOT NULL,
	[Perfil] [int] NOT NULL,
	[DataCriacao] [datetime] NOT NULL,
	[DataModificacao] [datetime] NULL,
	[UsuarioCriacao] [varchar](20) NOT NULL,
	[UsuarioModificacao] [varchar](20) NULL,
 CONSTRAINT [PK__TBUsuari__5E55825A7A112A1B] PRIMARY KEY CLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBUsuarioGrupo]    Script Date: 2023-04-01 10:46:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBUsuarioGrupo](
	[GrupoID] [uniqueidentifier] NOT NULL,
	[UsuarioID] [varchar](20) NOT NULL,
 CONSTRAINT [PK_UsuarioGrupo] PRIMARY KEY CLUSTERED 
(
	[GrupoID] ASC,
	[UsuarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TBAlerta] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[TBAlerta] ADD  DEFAULT (getdate()) FOR [DataCriacao]
GO
ALTER TABLE [dbo].[TBAlerta] ADD  CONSTRAINT [DF_TBAlerta_Atualizacao]  DEFAULT ((0)) FOR [Atualizacao]
GO
ALTER TABLE [dbo].[TBComentario] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[TBComentario] ADD  DEFAULT (getdate()) FOR [DataCriacao]
GO
ALTER TABLE [dbo].[TBGrupo] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[TBGrupo] ADD  DEFAULT (getdate()) FOR [DataCriacao]
GO
ALTER TABLE [dbo].[TBTag] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[TBTag] ADD  DEFAULT (getdate()) FOR [DataCriacao]
GO
ALTER TABLE [dbo].[TBTopico] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[TBTopico] ADD  DEFAULT (getdate()) FOR [DataCriacao]
GO
ALTER TABLE [dbo].[TBUsuario] ADD  CONSTRAINT [DF_TBUsuario_DataCriacao]  DEFAULT (getdate()) FOR [DataCriacao]
GO
ALTER TABLE [dbo].[TBAlerta]  WITH CHECK ADD FOREIGN KEY([TopicoID])
REFERENCES [dbo].[TBTopico] ([ID])
GO
ALTER TABLE [dbo].[TBAlerta]  WITH CHECK ADD  CONSTRAINT [FK__TBAlerta__Usuari__300424B4] FOREIGN KEY([UsuarioID])
REFERENCES [dbo].[TBUsuario] ([Login])
GO
ALTER TABLE [dbo].[TBAlerta] CHECK CONSTRAINT [FK__TBAlerta__Usuari__300424B4]
GO
ALTER TABLE [dbo].[TBComentario]  WITH CHECK ADD FOREIGN KEY([TopicoID])
REFERENCES [dbo].[TBTopico] ([ID])
GO
ALTER TABLE [dbo].[TBTopicoTag]  WITH CHECK ADD FOREIGN KEY([TagID])
REFERENCES [dbo].[TBTag] ([ID])
GO
ALTER TABLE [dbo].[TBTopicoTag]  WITH CHECK ADD FOREIGN KEY([TopicoID])
REFERENCES [dbo].[TBTopico] ([ID])
GO
ALTER TABLE [dbo].[TBUsuarioGrupo]  WITH CHECK ADD FOREIGN KEY([GrupoID])
REFERENCES [dbo].[TBGrupo] ([ID])
GO
ALTER TABLE [dbo].[TBUsuarioGrupo]  WITH CHECK ADD  CONSTRAINT [FK__TBUsuario__Usuar__412EB0B6] FOREIGN KEY([UsuarioID])
REFERENCES [dbo].[TBUsuario] ([Login])
GO
ALTER TABLE [dbo].[TBUsuarioGrupo] CHECK CONSTRAINT [FK__TBUsuario__Usuar__412EB0B6]
GO
USE [master]
GO
ALTER DATABASE [DB_KBFORUM] SET  READ_WRITE 
GO
