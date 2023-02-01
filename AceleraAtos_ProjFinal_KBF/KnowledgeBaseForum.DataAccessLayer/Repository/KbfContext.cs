using KnowledgeBaseForum.DataAccessLayer.Model;
using KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KnowledgeBaseForum.DataAccessLayer.Repository
{
    public class KbfContext : DbContext
    {
        private const string DATETIME_FORMAT = "datetime";
        private const string DATETIME_DEFAULT_VALUE = "getdate()";

        public DbSet<TopicoTag> AssociationTopicoTag { get; set; } = null!;
        public DbSet<UsuarioGrupo> AssociationUsuarioGrupo { get; set; } = null!;
        public DbSet<Alerta> Alertas { get; set; } = null!;
        public DbSet<Comentario> Comentarios { get; set; } = null!;
        public DbSet<Grupo> Grupos { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<Topico> Topicos { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;

        /// <summary>
        /// Constructor.
        /// </summary>
        //public KbfContext() : base()
        //{ }

        /// <summary>
        /// Constructor with DB options.
        /// </summary>
        public KbfContext(DbContextOptions options) : base(options) 
        { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TBUsuario
            EntityTypeBuilder<Usuario> usuarioBuilder = modelBuilder.Entity<Usuario>();
            usuarioBuilder.ToTable("TBUsuario").HasKey(u => u.Login);
            usuarioBuilder.Property(u => u.Login).HasMaxLength(20);
            usuarioBuilder.Property(u => u.Email).HasMaxLength(80).IsRequired();
            usuarioBuilder.Property(u => u.Nome).HasMaxLength(120).IsRequired();
            usuarioBuilder.Property(u => u.Senha).HasMaxLength(120).IsRequired();
            usuarioBuilder.Property(u => u.Perfil).IsRequired();
            usuarioBuilder.Property(u => u.DataCriacao).HasColumnType(DATETIME_FORMAT).IsRequired().HasDefaultValueSql(DATETIME_DEFAULT_VALUE);
            usuarioBuilder.Property(u => u.UsuarioCriacao).HasMaxLength(20).IsRequired();
            usuarioBuilder.Property(u => u.DataModificacao).HasColumnType(DATETIME_FORMAT);
            usuarioBuilder.Property(u => u.UsuarioModificacao).HasMaxLength(20);
            usuarioBuilder.HasMany(u => u.UsuarioGrupo)?.WithOne(ug => ug.Usuario).HasForeignKey(u => u.UsuarioId);

            // TBTag
            EntityTypeBuilder<Tag> tagBuilder = modelBuilder.Entity<Tag>();
            tagBuilder.ToTable("TBTag").HasKey(t => t.Id);
            tagBuilder.Property(t => t.Id).IsRequired();
            tagBuilder.Property(t => t.DataCriacao).HasColumnType(DATETIME_FORMAT).IsRequired().HasDefaultValueSql(DATETIME_DEFAULT_VALUE);
            tagBuilder.Property(t => t.UsuarioCriacao).HasMaxLength(20).IsRequired();
            tagBuilder.Property(t => t.DataModificacao).HasColumnType(DATETIME_FORMAT);
            tagBuilder.Property(t => t.Descricao).HasMaxLength(20).IsRequired();
            tagBuilder.HasMany(t => t.TopicoTag).WithOne(tt => tt.Tag).HasForeignKey(t => t.TagId);

            // TBTopico
            EntityTypeBuilder<Topico> topicoBuilder = modelBuilder.Entity<Topico>();
            topicoBuilder.ToTable("TBTopico").HasKey(t => t.Id);
            topicoBuilder.Property(t => t.Id).IsRequired();
            topicoBuilder.Property(t => t.DataCriacao).HasColumnType(DATETIME_FORMAT).IsRequired().HasDefaultValueSql(DATETIME_DEFAULT_VALUE);
            topicoBuilder.Property(t => t.UsuarioCriacao).HasMaxLength(20).IsRequired();
            topicoBuilder.Property(t => t.DataModificacao).HasColumnType(DATETIME_FORMAT);
            topicoBuilder.Property(t => t.UsuarioModificacao).HasMaxLength(20);
            topicoBuilder.Property(t => t.Titulo).HasMaxLength(60).IsRequired();
            topicoBuilder.Property(t => t.TipoAcesso).IsRequired();
            topicoBuilder.Property(t => t.Conteudo).IsRequired();
            topicoBuilder.Property(t => t.Status).HasDefaultValue(true).IsRequired();
            topicoBuilder.Property(t => t.UsuarioId).HasMaxLength(20).IsRequired();
            topicoBuilder.HasOne(t => t.Usuario).WithMany(u => u.Topicos).HasForeignKey(t => t.UsuarioId);
            topicoBuilder.HasMany(t => t.TopicoTag).WithOne(tt => tt.Topico).HasForeignKey(t => t.TopicoId);

            // TBComentario
            EntityTypeBuilder<Comentario> comentarioBuilder = modelBuilder.Entity<Comentario>();
            comentarioBuilder.ToTable("TBComentario").HasKey(c => c.Id);
            comentarioBuilder.Property(c => c.Id).IsRequired();
            comentarioBuilder.Property(c => c.DataCriacao).HasColumnType(DATETIME_FORMAT).IsRequired().HasDefaultValueSql(DATETIME_DEFAULT_VALUE);
            comentarioBuilder.Property(c => c.UsuarioCriacao).HasMaxLength(20).IsRequired();
            comentarioBuilder.Property(c => c.DataModificacao).HasColumnType(DATETIME_FORMAT);
            comentarioBuilder.Property(c => c.UsuarioModificacao).HasMaxLength(20);
            comentarioBuilder.Property(c => c.Status).HasDefaultValue(true).IsRequired();
            comentarioBuilder.Property(c => c.UsuarioId).HasMaxLength(20).IsRequired();
            comentarioBuilder.HasOne(c => c.Usuario).WithMany(u => u.Comentarios).HasForeignKey(c => c.UsuarioId);
            comentarioBuilder.Property(c => c.TopicoId).IsRequired();
            comentarioBuilder.HasOne(c => c.Topico).WithMany(t => t.Comentarios).HasForeignKey(c => c.TopicoId);
            comentarioBuilder.Property(c => c.Conteudo).HasMaxLength(400).IsRequired();

            // TBAlerta
            EntityTypeBuilder<Alerta> alertaBuilder = modelBuilder.Entity<Alerta>();
            alertaBuilder.ToTable("TBAlerta").HasKey(a => a.Id);
            alertaBuilder.Property(a => a.Id).IsRequired();
            alertaBuilder.Property(a => a.DataCriacao).HasColumnType(DATETIME_FORMAT).IsRequired().HasDefaultValueSql(DATETIME_DEFAULT_VALUE);
            alertaBuilder.Property(a => a.UsuarioCriacao).HasMaxLength(20).IsRequired();
            alertaBuilder.Property(a => a.DataModificacao).HasColumnType(DATETIME_FORMAT);
            alertaBuilder.Property(a => a.UsuarioModificacao).HasMaxLength(20);
            alertaBuilder.Property(a => a.ModoAlerta).IsRequired();
            alertaBuilder.Property(a => a.UsuarioId).HasMaxLength(20).IsRequired();
            alertaBuilder.HasOne(a => a.Usuario).WithMany(u => u.Alertas).HasForeignKey(a => a.UsuarioId);
            alertaBuilder.Property(a => a.TopicoId).IsRequired();
            alertaBuilder.HasOne(a => a.Topico).WithMany(t => t.Alertas).HasForeignKey(t => t.TopicoId);

            // TBGrupo
            EntityTypeBuilder<Grupo> grupoBuilder = modelBuilder.Entity<Grupo>();
            grupoBuilder.ToTable("TBGrupo").HasKey(t => t.Id);
            grupoBuilder.Property(g => g.Id).IsRequired();
            grupoBuilder.Property(g => g.DataCriacao).HasColumnType(DATETIME_FORMAT).IsRequired().HasDefaultValueSql(DATETIME_DEFAULT_VALUE);
            grupoBuilder.Property(g => g.UsuarioCriacao).HasMaxLength(20).IsRequired();
            grupoBuilder.Property(g => g.DataModificacao).HasColumnType(DATETIME_FORMAT);
            grupoBuilder.Property(g => g.UsuarioModificacao).HasMaxLength(20);
            grupoBuilder.Property(g => g.Status).HasDefaultValue(true).IsRequired();
            grupoBuilder.Property(g => g.Descricao).HasMaxLength(20).IsRequired();
            grupoBuilder.HasMany(g => g.UsuarioGrupo)?.WithOne(ug => ug.Grupo).HasForeignKey(u => u.GrupoId);

            // Association entities
            EntityTypeBuilder <UsuarioGrupo> ugBuilder = modelBuilder.Entity<UsuarioGrupo>();
            ugBuilder.ToTable("TBUsuarioGrupo").HasKey(ug => new { ug.GrupoId, ug.UsuarioId });
            ugBuilder.HasOne(ug => ug.Grupo).WithMany(g => g.UsuarioGrupo).HasForeignKey(ug => ug.GrupoId);
            ugBuilder.HasOne(ug => ug.Usuario).WithMany(u => u.UsuarioGrupo).HasForeignKey(ug => ug.UsuarioId);

            EntityTypeBuilder<TopicoTag> ttBuilder = modelBuilder.Entity<TopicoTag>();
            ttBuilder.ToTable("TBTopicoTag").HasKey(tt => new { tt.TagId, tt.TopicoId });
            ttBuilder.HasOne(tt => tt.Tag).WithMany(t => t.TopicoTag).HasForeignKey(tt => tt.TagId);
            ttBuilder.HasOne(tt => tt.Topico).WithMany(t => t.TopicoTag).HasForeignKey(tt => tt.TopicoId);
        }
    }
}
