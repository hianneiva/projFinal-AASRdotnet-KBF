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

        // API configuration keys
        public static readonly string API_AUTH_LOGIN = "ApiAuthLogin";
        public static readonly string API_AUTH_SIGNUP = "ApiAuthSignUp";
        public static readonly string API_COMENTARIO = "ApiComentario";
        public static readonly string API_HOST = "ApiHost";
        public static readonly string API_GRUPOS = "ApiGrupos";
        public static readonly string API_REL_TOPICO_TAG = "ApiRelationalTopicoTag";
        public static readonly string API_REL_USUARIO_GRUPO = "ApiRelationalUsuarioGrupo";
        public static readonly string API_TAGS = "ApiTags";
        public static readonly string API_TOPICO = "ApiTopico";
        public static readonly string API_USUARIO = "ApiUsuario";

        // Role names
        public static readonly string ROLE_ADMIN = "ADMIN";
        public static readonly string ROLE_NORMAL = "NORMAL";
    }
}
