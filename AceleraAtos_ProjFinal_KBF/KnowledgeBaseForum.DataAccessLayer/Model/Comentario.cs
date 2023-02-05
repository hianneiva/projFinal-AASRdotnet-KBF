namespace KnowledgeBaseForum.DataAccessLayer.Model
{
    public class Comentario : BaseEntity
    {
        /// <summary>
        /// Comment content.
        /// </summary>
        public string Conteudo { get; set; } = null!;

        /// <summary>
        /// Comment status (active).
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Topic Id.
        /// </summary>
        public Guid TopicoId { get; set; }

        /// <summary>
        /// Author (User) Id.
        /// </summary>
        public string UsuarioId { get; set; } = null!;

        // Relational properties.

        /// <summary>
        /// Author user.
        /// </summary>
        public Usuario? Usuario { get; set; } = null!;

        /// <summary>
        /// Parent topic.
        /// </summary>
        public Topico? Topico { get; set; } = null!;
    }
}
