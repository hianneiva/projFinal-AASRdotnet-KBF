namespace KnowledgeBaseForum.Commons.JWT.Model
{
    /// <summary>
    /// Class to implement the container for JWT specific configurations.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Name of the root object in the configuration file.
        /// </summary>
        public const string RootName = "Jwt";

        /// <summary>
        /// Expiration time in minutes.
        /// </summary>
        public int Expires { get; set; }

        /// <summary>
        /// The secret key to be used in the JWT creation/parsing.
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// The valid audience for the JWT token usage.
        /// </summary>
        public string ValidAudience { get; set; } = string.Empty;

        /// <summary>
        /// The valid issuer of the JWT token.
        /// </summary>
        public string ValidIssuer { get; set; } = string.Empty;
    }
}
