namespace KnowledgeBaseForum.Commons.Utils
{
    /// <summary>
    /// Define solution-wide constants.
    /// </summary>
    public class Const
    {
        // Configuration path keys to be used in setup/startup stage
        public const string JWT_SECRET = "Jwt:Secret";
        public const string JWT_VALID_AUDIENCE = "Jwt:ValidAudience";
        public const string JWT_VALID_ISSUER = "Jwt:ValidIssuer";

        // Role Constants
        public const string ROLE_ADMIN = "ADMIN";
        public const string ROLE_NORMAL = "NORMAL";
    }
}
