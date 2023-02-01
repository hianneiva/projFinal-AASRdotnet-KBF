namespace KnowledgeBaseForum.DataAccessLayer.Model
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// The row Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Creation date-time.
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Update date-time.
        /// </summary>
        public DateTime? DataModificacao { get; set; }

        /// <summary>
        /// Creation user name.
        /// </summary>
        public string UsuarioCriacao { get; set; } = null!;

        /// <summary>
        /// Update user name.
        /// </summary>
        public string? UsuarioModificacao { get; set; }
    }
}
