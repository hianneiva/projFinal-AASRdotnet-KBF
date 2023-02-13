namespace KnowledgeBaseForum.API.JWT
{
    /// <summary>
    /// Implements the basic model for JWT authentication.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Username for an user.
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Password for an user.
        /// </summary>
        public string Password { get; set; } = null!;
    }
}
