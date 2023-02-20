namespace KnowledgeBaseForum.AdminWebApp.Models.API
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

        /// <summary>
        /// Constructor.
        /// </summary>
        public Login()
        { }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        public Login(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
