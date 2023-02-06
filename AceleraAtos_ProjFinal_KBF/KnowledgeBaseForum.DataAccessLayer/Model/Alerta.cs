namespace KnowledgeBaseForum.DataAccessLayer.Model
{
    public class Alerta : BaseEntity
    {
        /// <summary>
        /// Alert mode (enum).
        /// </summary>
        public int ModoAlerta { get; set; }

        /// <summary>
        /// Author (User) Id.
        /// </summary>
        public string UsuarioId { get; set; } = null!;

        /// <summary>
        /// Topic Id.
        /// </summary>
        public Guid TopicoId { get; set; }

        // Relational properties.

        /// <summary>
        /// Topic to which the alert has been registered.
        /// </summary>
        public Topico? Topico { get; set; } = null!;

        /// <summary>
        /// Author user.
        /// </summary>
        public Usuario? Usuario { get; set; } = null!;
    }
}
