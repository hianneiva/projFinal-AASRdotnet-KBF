using KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel;

namespace KnowledgeBaseForum.DataAccessLayer.Model
{
    public class Topico : BaseEntity
    {
        /// <summary>
        /// Topic title.
        /// </summary>
        public string Titulo { get; set; } = null!;

        /// <summary>
        /// Topic content.
        /// </summary>
        public string Conteudo { get; set; } = null!;

        /// <summary>
        /// Access type (enum).
        /// </summary>
        public int TipoAcesso { get; set; }

        /// <summary>
        /// Topic status (active).
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Author (User) Id.
        /// </summary>
        public string UsuarioId { get; set; } = null!;

        // Relational properties

        /// <summary>
        /// Author user.
        /// </summary>
        public Usuario Usuario { get; set; } = null!;

        /// <summary>
        /// Topic comments.
        /// </summary>
        public IEnumerable<Comentario>? Comentarios { get; set; }

        /// <summary>
        /// Alerts registered to the topic.
        /// </summary>
        public IEnumerable<Alerta>? Alertas { get; set; }

        /// <summary>
        /// Topic-tag relation.
        /// </summary>
        public IEnumerable<TopicoTag>? TopicoTag { get; set; }
    }
}
