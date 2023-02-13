namespace KnowledgeBaseForum.AdminWebApp.Models.API
{
    /// <summary>
    /// Implements expected API response.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// If authentication was successful or not.
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// The authentication token as result of the JWT authentication process.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Contextual message.
        /// </summary>
        public string? Message { get; set; }
    }
}
