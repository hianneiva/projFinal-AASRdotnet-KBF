namespace KnowledgeBaseForum.API.Model
{
    /// <summary>
    /// Implements the basic model for JWT authentication response.
    /// </summary>
    public class JwtTokenResponse
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
