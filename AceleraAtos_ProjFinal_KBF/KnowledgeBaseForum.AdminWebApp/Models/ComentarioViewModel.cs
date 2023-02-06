namespace KnowledgeBaseForum.AdminWebApp.Models
{
    public class ComentarioViewModel : BaseViewModel
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

        /// <summary>
        /// Author data.
        /// </summary>
        public UsuarioViewModel? Usuario { get; set; }
    }
}
