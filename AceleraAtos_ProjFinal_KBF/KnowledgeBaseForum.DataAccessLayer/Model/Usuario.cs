using KnowledgeBaseForum.DataAccessLayer.Model.AssociationModel;

namespace KnowledgeBaseForum.DataAccessLayer.Model
{
    public class Usuario
    {
        /// <summary>
        /// User actual name.
        /// </summary>
        public string Nome { get; set; } = null!;

        /// <summary>
        /// User e-mail.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// User name used as login.
        /// </summary>
        public string Login { get; set; } = null!;

        /// <summary>
        /// User password.
        /// </summary>
        public string Senha { get; set; } = null!;

        /// <summary>
        /// User status (active).
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Current user profile.
        /// </summary>
        public int Perfil { get; set; }

        /// <summary>
        /// Creation date-time.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Update date-time.
        /// </summary>
        public DateTime? DataModificacao { get; set; }

        /// <summary>
        /// Creation user.
        /// </summary>
        public string UsuarioCriacao { get; set; } = null!;

        /// <summary>
        /// Update user.
        /// </summary>
        public string? UsuarioModificacao { get; set; }

        // Relational properties.

        /// <summary>
        /// Topic created by user.
        /// </summary>
        public IEnumerable<Topico>? Topicos { get; set; }

        /// <summary>
        /// User comments.
        /// </summary>
        public IEnumerable<Comentario>? Comentarios { get; set; }

        /// <summary>
        /// Alerts registered to the user.
        /// </summary>
        public IEnumerable<Alerta>? Alertas { get; set; }

        /// <summary>
        /// user-group relation.
        /// </summary>
        public IEnumerable<UsuarioGrupo>? UsuarioGrupo { get; set; }
    }
}
