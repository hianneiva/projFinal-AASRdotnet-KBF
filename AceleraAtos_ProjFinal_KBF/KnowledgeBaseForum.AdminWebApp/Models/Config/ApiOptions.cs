namespace KnowledgeBaseForum.AdminWebApp.Models.Config
{
    /// <summary>
    /// Implements the container for configuration data for API endpoints in setup/startup phase.
    /// </summary>
    public class ApiOptions
    {
        /// <summary>
        /// Name of the root object in the configuration file.
        /// </summary>
        public const string RootName = "API";

        /// <summary>
        /// The host of the API.
        /// </summary>
        public string ApiHost { get; set; } = string.Empty;

        /// <summary>
        /// The Login endpoint.
        /// </summary>
        public string ApiAuthLogin { get; set; } = string.Empty;

        /// <summary>
        /// The Comentario endpoint.
        /// </summary>
        public string ApiComentario { get; set; } = string.Empty;

        /// <summary>
        /// The Grupo endpoint.
        /// </summary>
        public string ApiGrupo { get; set; } = string.Empty;

        /// <summary>
        /// The relational Topico-Tag endpoint.
        /// </summary>
        public string ApiRelationalTopicoTag { get; set; } = string.Empty;

        /// <summary>
        /// The relational Usuario-Grupo endpoint.
        /// </summary>
        public string ApiRelationalUsuarioGrupo { get; set; } = string.Empty;

        /// <summary>
        /// The Tags endpoint.
        /// </summary>
        public string ApiTags { get; set; } = string.Empty;

        /// <summary>
        /// The Topico endpoint.
        /// </summary>
        public string ApiTopico { get; set; } = string.Empty;

        /// <summary>
        /// The Usuario endpoint.
        /// </summary>
        public string ApiUsuario { get; set; } = string.Empty;
    }
}
