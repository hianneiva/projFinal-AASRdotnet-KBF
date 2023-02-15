namespace KnowledgeBaseForum.AdminWebApp.Utils
{
    /// <summary>
    /// Class containing Constants to be used throught the project.
    /// </summary>
    public static class Constants
    {
        // System configuration keys
        public static readonly string STUB_MODE = "StubMode";
        public static readonly string JWT_SECRET = "JwtSecret";
        public static readonly string CYPHER_SECRET = "Cypher";

        // API configuration keys
        public static readonly string API_AUTH_LOGIN = "API:ApiAuthLogin";
        public static readonly string API_AUTH_SIGNUP = "API:ApiAuthSignUp";
        public static readonly string API_COMENTARIO = "API:ApiComentario";
        public static readonly string API_HOST = "API:ApiHost";
        public static readonly string API_GRUPOS = "API:ApiGrupos";
        public static readonly string API_REL_TOPICO_TAG = "API:ApiRelationalTopicoTag";
        public static readonly string API_REL_USUARIO_GRUPO = "API:ApiRelationalUsuarioGrupo";
        public static readonly string API_TAGS = "API:ApiTags";
        public static readonly string API_TOPICO = "API:ApiTopico";
        public static readonly string API_USUARIO = "API:ApiUsuario";

        // Role names
        public static readonly string ROLE_ADMIN = "ADMIN";
        public static readonly string ROLE_NORMAL = "NORMAL";
    }
}
