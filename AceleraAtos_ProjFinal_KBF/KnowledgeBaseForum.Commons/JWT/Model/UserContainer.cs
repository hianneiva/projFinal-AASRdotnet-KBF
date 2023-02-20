namespace KnowledgeBaseForum.Commons.JWT.Model
{
    /// <summary>
    /// Class to containerize user data for JWT token operations.
    /// </summary>
    public class UserContainer
    {
        /// <summary>
        /// The user's username.
        /// </summary>
        public string Username { get; set; } = string.Empty!;

        /// <summary>
        /// The user's given name.
        /// </summary>
        public string GivenName { get; set; } = string.Empty!;

        /// <summary>
        /// The user's profile value, used to define their role.
        /// </summary>
        public int Profile { get; set; }

        /// <summary>
        /// The user's e-mail.
        /// </summary>
        public string Email { get; set; } = string.Empty!;
    }
}
